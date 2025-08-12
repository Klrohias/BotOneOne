namespace BotOneOne.MessageFormat;

public class ReplyMessageSegment : MessageSegment<ReplyMessageSegment.Payload>
{
    public override string Type => "reply";

    public override string ToString()
    {
        return $"[Reply {Data.MessageId}]";
    }

    public ReplyMessageSegment(MessageId messageId)
    {
        Data = new Payload { MessageId = messageId };
    }

    public struct Payload
    {
        public MessageId MessageId { get; set; }
    }
}
