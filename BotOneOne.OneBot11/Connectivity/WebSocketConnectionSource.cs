using System.Net.WebSockets;

namespace BotOneOne.OneBot11.Connectivity;

public class WebSocketConnectionSource(string serverAddr) : BaseWebSocketConnection
{
    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _workerTask;
    private ClientWebSocket? _socket;

    public async Task Open(CancellationToken cancellationToken = default)
    {
        if (IsOpen)
        {
            return;
        }

        _socket = new ClientWebSocket();
        await _socket.ConnectAsync(new Uri(serverAddr), cancellationToken);
        Connection = _socket;

        _workerTask = Task.WhenAll(RxWorker(_cancellationTokenSource.Token),
            TxWorker(_cancellationTokenSource.Token));
    }

    public async Task Close(CancellationToken cancellationToken = default)
    {
        await _cancellationTokenSource.CancelAsync();
        _cancellationTokenSource = new CancellationTokenSource();
        Connection = null;
        await (_socket?.CloseAsync(WebSocketCloseStatus.NormalClosure, null, cancellationToken)
            ?? Task.CompletedTask);
        await (_workerTask ?? Task.CompletedTask);
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