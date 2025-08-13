namespace BotOneOne;

public readonly struct ChatId(object target)
{
    public object Target { get; } = target;

    public ChatId<T> AsTyped<T>() where T : notnull
    {
        return new ChatId<T>((T)Target);
    }
}


public readonly struct ChatId<T>(T target)
    where T : notnull
{
    public T Target { get; } = target;

    public static implicit operator ChatId(ChatId<T> @this)
    {
        return new ChatId(@this.Target);
    }
}
