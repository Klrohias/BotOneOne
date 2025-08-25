namespace BotEleven;

public static class Extensions
{
    /// <summary>
    /// 从 <see cref="IExtra"/> 取得额外信息
    /// </summary>
    public static TValue? GetExtra<TValue>(this IExtra extra, ExtraDefinition<TValue> definition)
    {
        return (TValue?)extra.GetExtra(definition.Key);
    }
    
    /// <summary>
    /// 从 <see cref="ChatId"/>（要求 Target 实现了 <see cref="IExtra"/>） 取得额外信息
    /// </summary>
    public static TValue? GetExtra<TValue>(this ChatId chatIdWithExtras, ExtraDefinition<TValue> value)
    {
        return chatIdWithExtras.AsTyped<IExtra>().Target.GetExtra(value);
    }

    /// <summary>
    /// 从 <see cref="ChatId"/>（要求 Target 实现了 <see cref="IExtra"/>） 取得额外信息
    /// </summary>
    public static TValue? GetExtra<TValue, T>(this ChatId<T> chatIdWithExtras, ExtraDefinition<TValue> value)
        where T : IExtra
    {
        return chatIdWithExtras.Target.GetExtra(value);
    }
}