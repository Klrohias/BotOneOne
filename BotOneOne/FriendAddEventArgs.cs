namespace BotOneOne;

/// <summary>
/// 添加好友事件参数
/// </summary>
public class FriendAddEventArgs
{
    /// <summary>
    /// 引起事件发生的用户（想要添加你为好友的用户）
    /// </summary>
    public ChatId User { get; set; }
    
    /// <summary>
    /// 添加好友的验证消息
    /// </summary>
    public string? Comment { get; set; }
    
    /// <summary>
    /// 该事件的反馈 Id，需要同意/拒绝加群请求时使用
    /// </summary>
    public FeedbackId FeedbackId { get; set; }
    
    /// <summary>
    /// 该事件的发生时间
    /// </summary>
    public DateTimeOffset Time { get; set; }
}