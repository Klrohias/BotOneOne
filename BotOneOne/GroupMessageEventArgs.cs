using BotOneOne.MessageFormat;

namespace BotOneOne;

/// <summary>
/// 群聊事件参数
/// </summary>
public class GroupMessageEventArgs
{
    /// <summary>
    /// 发生事件的群组（该条消息的所在群聊）
    /// </summary>
    public ChatId Group { get; set; }
    
    /// <summary>
    /// 引起事件发生的用户（消息的发送者）
    /// </summary>
    public ChatId User { get; set; }
    
    /// <summary>
    /// 接收到的消息
    /// </summary>
    public Message Message { get; set; } = Message.Empty;
    
    
    /// <summary>
    /// 该条消息的 Id
    /// </summary>
    public MessageId MessageId { get; set; }
    
    /// <summary>
    /// 该条消息的发送时间
    /// </summary>
    public DateTimeOffset Time { get; set; }
}