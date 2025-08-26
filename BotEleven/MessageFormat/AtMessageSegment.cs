namespace BotEleven.MessageFormat;

public class AtMessageSegment(ChatId target) : MessageSegment
{
    public override string Type => "at";
    public ChatId Target { get; set; } = target;

    public override string ToString()
    {
        return $"[At {Target}]";
    }
}
