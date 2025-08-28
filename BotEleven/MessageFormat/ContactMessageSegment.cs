namespace BotEleven.MessageFormat;

public class ContactMessageSegment(ChatId target) : MessageSegment
{
    public override string Type => "contact";
    public ChatId Target { get; set; } = target;
}