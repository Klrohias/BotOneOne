namespace BotEleven.MessageFormat;

public class AtMessageSegment : MessageSegment<AtMessageSegment.Payload>
{
    public override string ToString()
    {
        return $"[At {Data.Target}]";
    }

    public AtMessageSegment(ChatId target)
    {
        Data = new Payload
        {
            Target = target
        };
    }

    public struct Payload
    {
        public ChatId Target { get; set; }
    }

    public override string Type => "at";
}
