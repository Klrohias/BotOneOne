namespace BotOneOne;

/// <summary>
/// 额外信息的定义
/// </summary>
/// <param name="key">额外信息的键</param>
/// <typeparam name="TValue">额外信息的类型</typeparam>
public readonly struct ExtraDefinition<TValue>(string key)
{
    /// <summary>
    /// 额外信息的键
    /// </summary>
    public string Key { get; } = key;
}