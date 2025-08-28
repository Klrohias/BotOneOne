namespace BotEleven.OneBot11;

/// <summary>
/// 有关 OneBot11 的选项
/// </summary>
public struct OneBot11Options(TimeSpan invocationTimeout, short workerId)
{
    /// <summary>
    /// 动作调用的超时，默认为 30 秒
    /// </summary>
    public TimeSpan InvocationTimeout { get; set; } = invocationTimeout;

    /// <summary>
    /// 节点Id，若在集群环境下对同一服务器运行 Bot 则建议改动该值，范围在 0~1023，默认为 0
    /// </summary>
    public short WorkerId { get; set; } = workerId;

    public OneBot11Options()
        : this(invocationTimeout: TimeSpan.FromSeconds(30),
            workerId: 0)
    {
    }

    /// <summary>
    /// 取得一个默认的选项
    /// </summary>
    public static readonly OneBot11Options Default = new();
}