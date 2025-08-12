using System.Net;

namespace BotOneOne.OneBot11.Connectivity;

public class ReversedWebSocketListener(ReversedWebSocketConnectionSource connectionSource)
{
    private Task? _workerTask;
    private CancellationTokenSource _cancellationTokenSource = new();
    public HttpListener Listener { get; } = new();
    public event Action? WebSocketConnected;

    public ReversedWebSocketListener AddPrefix(string prefix)
    {
        Listener.Prefixes.Add(prefix);
        return this;
    }

    public ReversedWebSocketListener RemovePrefix(string prefix)
    {
        Listener.Prefixes.Remove(prefix);
        return this;
    }

    public void Start()
    {
        if (_workerTask != null)
        {
            return;
        }

        Listener.Start();
        _workerTask = Worker(_cancellationTokenSource.Token);
    }

    public void Stop()
    {
        Listener.Stop();
        connectionSource.Close();
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        _workerTask = null;
    }

    private async Task Worker(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var context = await Listener.GetContextAsync();
            var webSocketCtx = await context.AcceptWebSocketAsync(null);
            var task = connectionSource.UpgradeWebSocket(webSocketCtx.WebSocket);
            WebSocketConnected?.Invoke();
            
            await (task ?? Task.CompletedTask);
        }
    }
}