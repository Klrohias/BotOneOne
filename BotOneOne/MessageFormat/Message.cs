namespace BotOneOne.MessageFormat;

public class Message
{
    public static Message Empty { get; } = new();

    public List<MessageSegment> Segments { get; } = [];

    public Message Append(MessageSegment segment)
    {
        Segments.Add(segment);
        return this;
    }

    public Message Append(Message other)
    {
        Segments.AddRange(other.Segments);
        return this;
    }

    public override string ToString()
    {
        return string.Join(' ', Segments.Select(x => x.ToString()));
    }
}
