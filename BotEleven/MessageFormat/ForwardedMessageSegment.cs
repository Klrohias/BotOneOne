namespace BotEleven.MessageFormat;

public class ForwardedMessageSegment(MessageId contentId) : MessageSegment
{
    public override string Type => "forwarded";
    public MessageId ContentId { get; set; } = contentId;
}