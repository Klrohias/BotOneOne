namespace BotEleven.MessageFormat;

public abstract class MessageSegment
{
    public abstract string Type { get; }
}

public abstract class MessageSegment<T> : MessageSegment
{
    public T? Data { get; set; }
}
