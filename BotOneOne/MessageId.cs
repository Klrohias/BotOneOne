namespace BotOneOne;

public readonly struct MessageId(object target)
{
    public object Target { get; } = target;

    public MessageId<T> AsTyped<T>() where T : notnull
    {
        return new MessageId<T>((T)Target);
    }
}

public readonly struct MessageId<T>(T target)
    where T : notnull
{
    public T Target { get; } = target;

    public static implicit operator MessageId(MessageId<T> @this)
    {
        return new MessageId(@this.Target);
    }
}
