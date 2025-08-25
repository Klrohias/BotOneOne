using BotOneOne.MessageFormat;

namespace BotOneOne;

/// <summary>
/// 消息的详细信息
/// </summary>
/// <param name="time">消息发送的时间</param>
/// <param name="sender">消息发送者</param>
/// <param name="message">消息内容</param>
public class MessageDetail(DateTimeOffset time, ChatId sender, Message message)
{
    /// <summary>
    /// 消息发送的时间
    /// </summary>
    public DateTimeOffset? Time { get; set; } = time;
    
    /// <summary>
    /// 消息发送者
    /// </summary>
    public ChatId Sender { get; set; } = sender;
    
    /// <summary>
    /// 消息内容
    /// </summary>
    public Message Message { get; set; } = message;
}