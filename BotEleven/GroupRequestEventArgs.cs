namespace BotEleven;

/// <summary>
/// 「加群请求」或「拉入群邀请」的事件参数
/// </summary>
public class GroupRequestEventArgs
{
    /// <summary>
    /// 发生事件的群组（用户申请加的群，或 Bot 被拉入的群）
    /// </summary>
    public ChatId Group { get; set; }
    
    /// <summary>
    /// 引起事件发生的用户（申请加群的用户，或拉 Bot 进群的用户）
    /// </summary>
    public ChatId User { get; set; }
    
    /// <summary>
    /// 加群的验证信息（只有加群请求事件提供）
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