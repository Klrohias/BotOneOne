namespace BotEleven.MessageFormat;

public class ReplyMessageSegment(MessageId messageId) : MessageSegment
{
    public override string Type => "reply";
    public MessageId MessageId { get; set; } = messageId;

    public override string ToString()
    {
        return $"[Reply {MessageId}]";
    }
}
