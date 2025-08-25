namespace BotEleven;

/// <summary>
/// 支持以一个 string key 获取额外信息的对象
/// </summary>
public interface IExtra
{
    /// <summary>
    /// 获取对象的额外信息
    /// </summary>
    /// <param name="key">所获取信息的键</param>
    /// <returns>获取到的信息</returns>
    /// <exception cref="NotSupportedException">这一对象不支持获取额外信息</exception>
    public object? GetExtra(string key);
}
