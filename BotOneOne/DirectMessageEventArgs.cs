using BotOneOne.MessageFormat;

namespace BotOneOne;

public class DirectMessageEventArgs
{
    public ChatId User { get; set; }
    public Message Message { get; set; } = Message.Empty;
    public MessageId MessageId { get; set; }
    public DateTimeOffset Time { get; set; }
}