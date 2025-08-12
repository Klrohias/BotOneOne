namespace BotOneOne;

public struct MessageId
{
    public object Target { get; }

    public MessageId(object target)
    {
        Target = target;
    }

    public MessageId<T> IntoTransparent<T>() where T : notnull
    {
        return new MessageId<T>((T)Target);
    }
}

public struct MessageId<T>
    where T : notnull
{
    public T Target { get; }

    public MessageId(T target)
    {
        Target = target;
    }

    public MessageId IntoOpaque()
    {
        return new MessageId(Target);
    }
}
