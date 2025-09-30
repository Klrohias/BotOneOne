namespace BotEleven.Milky;

public class MilkyOptions
{
    /// <summary>
    /// Milky 服务端的 Token（令牌）
    /// </summary>
    public string? Token { get; set; }
    
    /// <summary>
    /// 动作调用的超时，默认为 30s
    /// </summary>
    public TimeSpan InvocationTimeout { get; set; }
    
    /// <summary>
    /// 断线重连的间隔，为 null 时断线不重连，默认为 null
    /// </summary>
    public TimeSpan? ReconnectInterval { get; set; }

    public static MilkyOptions Default => new()
    {
        InvocationTimeout = TimeSpan.FromSeconds(30),
    };
}
