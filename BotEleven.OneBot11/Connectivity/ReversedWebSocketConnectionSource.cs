using System.Net.WebSockets;

namespace BotEleven.OneBot11.Connectivity;

/// <summary>
/// OneBot 11 WebSocket 反向连接的连接源
/// </summary>
public class ReversedWebSocketConnectionSource : BaseWebSocketConnection
{
    private CancellationTokenSource _cancellationTokenSource = new();

    /// <summary>
    /// Set the active socket, and task of workers will be returned.
    /// If the connection is already opened, null will be returned.
    /// </summary>
    /// <returns>A task which will be completed when connection close</returns>
    public Task? UpgradeWebSocket(WebSocket socket)
    {
        if (IsOpen)
        {
            return null;
        }

        Connection = socket;
        
        return Task.WhenAll(RxWorker(_cancellationTokenSource.Token),
            TxWorker(_cancellationTokenSource.Token));
    }

    public void Close()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        Connection = null;
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