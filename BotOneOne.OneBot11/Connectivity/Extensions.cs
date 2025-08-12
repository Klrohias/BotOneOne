using System.Text;

namespace BotOneOne.OneBot11.Connectivity;

public static class Extensions
{
    public static async Task<string> ReceiveTextAsync(this IConnectionSource connectionSource,
        CancellationToken cancellationToken = default)
    {
        var result = await connectionSource.ReadPacket(cancellationToken);
        return Encoding.Default.GetString(result.ToArray());
    }

    public static Task SendTextAsync(this IConnectionSource connectionSource, string text,
        CancellationToken cancellationToken = default)
    {
        var packet = Encoding.Default.GetBytes(text);
        return connectionSource.SendPacket(packet, cancellationToken);
    }
}