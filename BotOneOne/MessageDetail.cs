using BotOneOne.MessageFormat;

namespace BotOneOne;

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