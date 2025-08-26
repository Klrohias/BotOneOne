using System.Net.WebSockets;
using System.Threading.Tasks.Dataflow;

namespace BotEleven.OneBot11.Connectivity;

public abstract class BaseWebSocketConnection : IConnectionSource
{
    private readonly BufferBlock<Memory<byte>> _receiveQueue = new();
    private readonly BufferBlock<Memory<byte>> _sendQueue = new();

    protected WebSocket? Connection { get; set; }
    public virtual bool IsOpen => Connection is { State: WebSocketState.Open };

    public Task<Memory<byte>> ReadPacket(CancellationToken cancellationToken)
    {
        return _receiveQueue.ReceiveAsync(cancellationToken);
    }

    public Task SendPacket(Memory<byte> packet, CancellationToken cancellationToken)
    {
        return _sendQueue.SendAsync(packet, cancellationToken);
    }

    /// <summary>
    /// Poll Rx, call when `IsOpen == true` or it will cause exceptions.
    /// </summary>
    protected async ValueTask PollRx(CancellationToken cancellationToken)
    {
        Memory<byte> buffer = new byte[1024];
        using var packetStream = new MemoryStream();

        ValueWebSocketReceiveResult receiveResult;
        do
        {
            receiveResult = await Connection!.ReceiveAsync(buffer, cancellationToken);
            await packetStream.WriteAsync(buffer[..receiveResult.Count], cancellationToken);
        } while (!receiveResult.EndOfMessage);

        _receiveQueue.Post(packetStream.ToArray());
    }

    /// <summary>
    /// Poll Tx, call when `IsOpen == true` or it will cause exceptions.
    /// </summary>
    protected async ValueTask PollTx(CancellationToken cancellationToken)
    {
        var sendPacket = await _sendQueue.ReceiveAsync(cancellationToken);
        await Connection!.SendAsync(sendPacket, WebSocketMessageType.Text, true, cancellationToken);
    }
}