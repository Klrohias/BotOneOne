namespace BotOneOne.MessageFormat;

public static class Extensions
{
    public static Message Text(this Message message, string content)
    {
        message.Append(new TextMessageSegment(content));
        return message;
    }

    public static Message At(this Message message, ChatId target)
    {
        message.Append(new AtMessageSegment(target));
        return message;
    }
    
    public static Message Image(this Message message, string file)
    {
        message.Append(new ImageMessageSegment(file));
        return message;
    }
    
    public static Message Reply(this Message message, MessageId messageId)
    {
        message.Append(new ReplyMessageSegment(messageId));
        return message;
    }
}