using BotEleven.MessageFormat;

namespace BotEleven;

/// <summary>
/// 私聊事件参数
/// </summary>
public class DirectMessageEventArgs
{
    /// <summary>
    /// 引起事件发生的用户（消息的发送者）
    /// </summary>
    public ChatId User { get; set; }
    
    /// <summary>
    /// 接收到的消息
    /// </summary>
    public Message Message { get; set; } = Message.Empty;
    
    /// <summary>
    /// 接收到的消息 Id
    /// </summary>
    public MessageId MessageId { get; set; }
    
    /// <summary>
    /// 该条消息发送的时间
    /// </summary>
    public DateTimeOffset Time { get; set; }
}