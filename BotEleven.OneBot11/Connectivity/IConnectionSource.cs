namespace BotEleven.OneBot11.Connectivity;

/// <summary>
/// OneBot 11 连接源接口
/// </summary>
public interface IConnectionSource
{
    public Task<Memory<byte>> ReadPacket(CancellationToken cancellationToken);
    public Task SendPacket(Memory<byte> packet, CancellationToken cancellationToken);
}