namespace BotOneOne.OneBot11.Entities;

public struct User
{
    /// <summary>
    /// 用户号码
    /// </summary>
    public long Id;

    /// <summary>
    /// 用户名称
    /// </summary>
    public string? Name;
    
    /// <summary>
    /// 对该用户的备注
    /// </summary>
    public string? Remark;
    
    /// <summary>
    /// 该用户的性别
    /// </summary>
    public string? Sex;
    
    /// <summary>
    /// 该用户的性别
    /// </summary>
    public int? Age;

    /// <summary>
    /// 该用户在群中的等级. <br />
    /// <b>仅在 event GroupMessage 的参数提供</b>
    /// </summary>
    public int? GroupLevel;
    
    /// <summary>
    /// 用户在群中的名片.<br />
    /// <b>仅在 event GroupMessage 的参数提供</b>
    /// </summary>
    public string? GroupMask;

    /// <summary>
    /// 用户在群中的职位，0 = 普通群员，1 = 管理员，2 = 群组，3 = 其他. <br />
    /// <b>仅在 event GroupMessage 的参数提供</b>
    /// </summary>
    public int? GroupRole;
    
    /// <summary>
    /// 用户在群中的头衔.<br />
    /// <b>仅在 event GroupMessage 的参数提供</b>
    /// </summary>
    public string? GroupHonor;

    /// <summary>
    /// 创建 User
    /// </summary>
    public static User Of(long id,
        string? name = null,
        string? groupMask = null,
        int? groupRole = null,
        string? remark = null,
        string? sex = null,
        int? age = null,
        int? groupLevel = null,
        string? groupHonor = null)
    {
        return new User
        {
            Id = id,
            Name = name,
            GroupMask = groupMask,
            GroupRole = groupRole,
            Remark = remark,
            Age = age,
            Sex = sex,
            GroupLevel = groupLevel,
            GroupHonor = groupHonor
        };
    }
}

public static partial class Extensions
{
    public static ChatId AsChatId(this User user)
    {
        return new ChatId(user);
    }

    public static bool IsUser(this ChatId chatId)
    {
        return chatId.Target is User;
    }
    
    public static User AsUser(this ChatId chatId)
    {
        return chatId.IntoTransparent<User>().Target;
    }
}
