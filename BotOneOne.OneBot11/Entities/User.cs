namespace BotOneOne.OneBot11.Entities;

public readonly struct User
{
    /// <summary>
    /// QQ 号
    /// </summary>
    public readonly long Id;

    /// <summary>
    /// 用户的额外信息，仅从框架得出（事件参数/方法返回值）的 User 对象可能会有
    /// </summary>
    public readonly UserExtra? Extra;

    private User(long id, UserExtra? extra)
    {
        Id = id;
        Extra = extra;
    }
    
    /// <summary>
    /// 创建 User
    /// </summary>
    public static User Of(long id, UserExtra? extra = null)
    {
        return new User(id, extra);
    }
}

/// <summary>
/// 用户的额外数据
/// </summary>
/// <param name="Name">用户昵称</param>
/// <param name="GroupMask">用户群名片</param>
/// <param name="GroupRole">用户群地位 (0 = 普通群员, 1 = 管理员, 2 = 群主, 3 = 未知/其他)</param>
/// <param name="Remark">对该用户的备注</param>
/// <param name="Sex">用户的性别 (0 = 女, 1 = 男, 2 = 未知, 3 = 其他)</param>
/// <param name="Age">用户的年龄</param>
/// <param name="GroupLevel">用户的群等级</param>
/// <param name="GroupHonor">用户的群头衔</param>
/// <param name="GroupJoinTime">用户加入该群的时间</param>
/// <param name="GroupLastSentTime">用户最后一次在该群发消息的时间</param>
public record UserExtra(
    string? Name = null,
    string? GroupMask = null,
    int? GroupRole = null,
    string? Remark = null,
    int? Sex = null,
    int? Age = null,
    int? GroupLevel = null,
    string? GroupHonor = null,
    DateTimeOffset? GroupJoinTime = null,
    DateTimeOffset? GroupLastSentTime = null);

public static partial class Extensions
{
    public static ChatId<User> AsChatId(this User user)
    {
        return new ChatId<User>(user);
    }

    public static bool IsUser(this ChatId chatId)
    {
        return chatId.Target is User;
    }
    
    public static User AsUser(this ChatId chatId)
    {
        return chatId.AsTyped<User>().Target;
    }
}
