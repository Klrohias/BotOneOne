using BotEleven.Modules;
using BotEleven.Standard;
using Newtonsoft.Json;

namespace BotEleven.OneBot11.Entities;

/// <summary>
/// 代表一个用户的对象
/// </summary>
/// <param name="Id">QQ号</param>
/// <param name="Extra">额外信息</param>
public record User(long Id, UserExtra? Extra) : IExtra
{
    /// <summary>
    /// 构造一个 User 对象，与 new 基本无异
    /// </summary>
    public static User Of(long id, UserExtra? extra = null)
    {
        return new User(id, extra);
    }

    public object? GetExtra(string key)
    {
        if (key == nameof(UserExtras.Id))
        {
            return Id;
        }
        
        if (Extra is null) throw new NotSupportedException("Not supported on this object");
        return key switch
        {
            nameof(UserExtras.Nickname) => Extra.Name,
            _ => throw new NotSupportedException($"Extra \"{key}\" is not supported")
        };
    }

    static User()
    {
        ModuleManager.RegisterSerializableType<User>("onebot11-user");
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