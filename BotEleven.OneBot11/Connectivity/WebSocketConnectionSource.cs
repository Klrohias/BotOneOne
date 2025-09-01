using System.Net.WebSockets;
using BotEleven.OneBot11.Internals;

namespace BotEleven.OneBot11.Connectivity;

/// <summary>
/// OneBot 11 WebSocket 正向连接的连接源
/// </summary>
/// <param name="serverAddr">WebSocket 服务器地址</param>
/// <param name="token">连接服务器时需要的 Token，null 为不需要，空字符串为提供空 Token，默认为 null</param>
/// <param name="reconnectInterval">服务器断线重连间隔，null 为不重连，默认为 null</param>
public class WebSocketConnectionSource(string serverAddr, string? token = null, TimeSpan? reconnectInterval = null)
    : BaseWebSocketConnection
{
    /// <summary>
    /// 连接服务器时需要的 Token，null 为不需要，空字符串为提供空 Token，默认为 null
    /// </summary>
    public string? Token { get; set; } = token;
    
    /// <summary>
    /// 服务器断线重连间隔，null 为不重连，默认为 null
    /// </summary>
    public TimeSpan? ReconnectInterval { get; set; } = reconnectInterval;
    
    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _workerTask;
    private int _selfOpened;

    public override bool IsOpen => _selfOpened == 1 && base.IsOpen;

    public void Open()
    {
        if (Interlocked.CompareExchange(ref _selfOpened, 1, 0) == 1)
        {
            return;
        }

        _selfOpened = 1;
        _workerTask = Worker(_cancellationTokenSource.Token);
    }

    public async Task Close(CancellationToken cancellationToken = default)
    {
        if (Interlocked.CompareExchange(ref _selfOpened, 0, 1) == 0)
        {
            return;
        }
        
#if NET8_0_OR_GREATER
        await _cancellationTokenSource.CancelAsync();
#else
        _cancellationTokenSource.Cancel();
#endif
        
        _cancellationTokenSource = new CancellationTokenSource();
        
        await (Connection?.CloseAsync(WebSocketCloseStatus.NormalClosure, null, cancellationToken) ?? Task.CompletedTask);
        await (_workerTask ?? Task.CompletedTask);
        Connection = null;
    }

    private async Task Worker(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await AutoReconnect(cancellationToken);
                await Task.WhenAll(RxWorker(_cancellationTokenSource.Token),
                    TxWorker(_cancellationTokenSource.Token));
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                
                if (ReconnectInterval == null)
                {
                    return;
                }

                await Task.Delay(ReconnectInterval.Value, cancellationToken);
            }
        }
    }

    private async ValueTask AutoReconnect(CancellationToken cancellationToken)
    {
        var query = Token is not null ? $"?access_token={Token}" : string.Empty;
        var socket = new ClientWebSocket();
        await socket.ConnectAsync(new Uri($"{serverAddr}{query}"), cancellationToken);
        Connection = socket;
    }

    private async Task RxWorker(CancellationToken cancellationToken)
    {
        while (IsOpen && !cancellationToken.IsCancellationRequested)
        {
            await PollRx(cancellationToken);
        }
    }

    private async Task TxWorker(CancellationToken cancellationToken)
    {
        while (IsOpen && !cancellationToken.IsCancellationRequested)
        {
            await PollTx(cancellationToken);
        }
    }
}