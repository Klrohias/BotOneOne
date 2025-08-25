namespace BotOneOne.Standard;

public static class UserExtras
{
    /// <summary>
    /// 用户的号码（QQ号）
    /// </summary>
    public static readonly ExtraDefinition<long> Id = new(nameof(Id));
    
    /// <summary>
    /// 用户的昵称
    /// </summary>
    public static readonly ExtraDefinition<string> Nickname = new(nameof(Nickname));
}