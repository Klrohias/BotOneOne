using System.Net.Http.Headers;
using System.Net.WebSockets;
using BotEleven.Milky.Internals;
using BotEleven.Milky.Transfer;
using Newtonsoft.Json;

namespace BotEleven.Milky;

internal class Dialer(Uri endpoint, MilkyOptions options)
{
    private readonly HttpClient _httpClient = new();
    private readonly Lock _lock = new();
    
    private ClientWebSocket _clientWebSocket = null!;
    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _workerTask;

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
                await Task.Delay(options.ReconnectInterval, cancellationToken);
            }
        }
    }

    private async ValueTask Receiver(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var packet = await ReceivePacket(cancellationToken);
            
            // TODO
        }
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