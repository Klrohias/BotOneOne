namespace BotOneOne;

public struct ChatId
{
    public object Target { get; }

    public ChatId(object target)
    {
        Target = target;
    }

    public ChatId<T> IntoTransparent<T>() where T : notnull
    {
        return new ChatId<T>((T)Target);
    }
}


public struct ChatId<T>
    where T : notnull
{
    public T Target { get; }

    public ChatId(T target)
    {
        Target = target;
    }

    public ChatId IntoOpaque()
    {
        return new ChatId(Target);
    }
}
