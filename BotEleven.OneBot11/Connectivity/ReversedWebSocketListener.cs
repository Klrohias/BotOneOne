using System.Net;

namespace BotEleven.OneBot11.Connectivity;

/// <summary>
/// OneBot 11 WebSocket 反向连接的 Listener 服务器，基于 <see cref="HttpListener"/>，可快速启动一个 WebSocket 服务器供反向连接使用 <br />
/// </summary>
/// <param name="connectionSource">反向连接源，当此 Listener 服务器接收到后端的 WebSocket 连接后会反弹到反向连接源上</param>
/// <param name="token">后端连接所需的 Token，若为 null 或空字符串代表无需 Token</param>
public class ReversedWebSocketListener(ReversedWebSocketConnectionSource connectionSource, string? token = null)
{
    /// <summary>
    /// 后端连接所需的 Token，若为 null 或空字符串代表无需 Token
    /// </summary>
    public string? Token { get; set; } = token;
    
    private Task? _workerTask;
    private CancellationTokenSource _cancellationTokenSource = new();
    
    /// <summary>
    /// 使用的 Listener 
    /// </summary>
    public HttpListener Listener { get; } = new();
    
    /// <summary>
    /// 当接收到连接并反弹至反向连接源后触发
    /// </summary>
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
            if (!string.IsNullOrWhiteSpace(Token))
            {
                var authHeader = context.Request.Headers["Authorization"];
                if (string.IsNullOrWhiteSpace(authHeader))
                {
                    context.Response.StatusCode = 401;
                    context.Response.Close();
                    continue;
                }

                if (authHeader.ToLower().StartsWith("bearer "))
                {
                    authHeader = authHeader[7..];
                }

                if (authHeader != Token)
                {
                    context.Response.StatusCode = 401;
                    context.Response.Close();
                    continue;
                }
            }

            var webSocketCtx = await context.AcceptWebSocketAsync(null);
            var task = connectionSource.UpgradeWebSocket(webSocketCtx.WebSocket);
            WebSocketConnected?.Invoke();
            
            await (task ?? Task.CompletedTask);
        }
    }
}