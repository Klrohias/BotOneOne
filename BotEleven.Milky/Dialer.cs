using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using BotEleven.Milky.Internals;
using BotEleven.Milky.Transfer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotEleven.Milky;

internal class Dialer(Uri endpoint, MilkyOptions options, Action<Event<JToken>>? onEventReceived = null)
{
    private readonly HttpClient _httpClient = new();
    private readonly Lock _lock = new();
    
    private ClientWebSocket _clientWebSocket = null!;
    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _workerTask;

    public Action<Event<JToken>>? OnEventReceived { get; set; } = onEventReceived;

    private static readonly MediaTypeHeaderValue ActionMediaType = MediaTypeHeaderValue.Parse("application/json");
    
    public bool Opened { get; private set; }

    public void Open()
    {
        lock (_lock)
        {
            Opened = true;
            _workerTask = Worker(_cancellationTokenSource.Token);
        }
    }

    public void Close()
    {
        lock (_lock)
        {
            Opened = false;
            _cancellationTokenSource.Cancel();
            _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            _workerTask?.Wait();
            
            _workerTask = null;
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }

    private async Task Worker(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await AutoReconnect(cancellationToken);
                await Receiver(cancellationToken);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                if (!options.ReconnectInterval.HasValue)
                {
                    return;
                }
                
                await Task.Delay(options.ReconnectInterval.Value, cancellationToken);
            }
        }
    }

    private async ValueTask Receiver(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var rawPacket = await ReceivePacket(cancellationToken);
            
            try
            {
                ReceiveEvent(rawPacket);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }
    }

    private void ReceiveEvent(byte[] rawPacket)
    {
        var packetString = Encoding.Default.GetString(rawPacket);
        var packet = JsonConvert.DeserializeObject<Event<JToken>>(packetString);
        if (packet == null)
        {
            return;
        }
        
        OnEventReceived?.Invoke(packet);
    }

    private async ValueTask<byte[]> ReceivePacket(CancellationToken cancellationToken)
    {
        Memory<byte> buffer = new byte[1024];
        using var packetStream = new MemoryStream();

        ValueWebSocketReceiveResult receiveResult;
        do
        {
            receiveResult = await _clientWebSocket.ReceiveAsync(buffer, cancellationToken);
            await packetStream.WriteAsync(buffer[..receiveResult.Count], cancellationToken);
        } while (!receiveResult.EndOfMessage);

        return packetStream.ToArray();
    }

    private async ValueTask AutoReconnect(CancellationToken cancellationToken)
    {
        var query = options.Token is not null ? $"?access_token={options.Token}" : string.Empty;
        _clientWebSocket = new ClientWebSocket();
        await _clientWebSocket.ConnectAsync(new Uri(endpoint, $"event{query}"), cancellationToken);
    }

    private HttpRequestMessage BuildRequest(Uri uri, object data)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = uri,
            Method = HttpMethod.Post
        };
        
        // access token
        if (options.Token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);
        }
        
        // content
        request.Content = new StringContent(JsonConvert.SerializeObject(data), ActionMediaType);

        return request;
    }

    public async Task<ActionResponse<T>?> InvokeAction<T>(string path, object? data, CancellationToken cancellationToken)
    {
        var uri = new Uri(endpoint, $"api{path}");
        var request = BuildRequest(uri, data ?? new object());
        
        // request
        var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<ActionResponse<T>>(responseText);
    }
}