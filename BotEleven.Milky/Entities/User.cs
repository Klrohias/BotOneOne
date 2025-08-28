using BotEleven.Modules;
using BotEleven.Standard;

namespace BotEleven.Milky.Entities;

/*
 * From https://milky.ntqqrev.org/struct/FriendEntity
 */

/*
字段名	类型	描述
user_id	 int64	用户 QQ 号
nickname	string	用户昵称
sex	"male" | "female" | "unknown"	用户性别
qid	string	用户 QID
remark	string	好友备注
category	FriendCategoryEntity	好友分组
 */

/// <summary>
/// 用户实体
/// </summary>
public class User : IExtra
{
    /// <summary>
    /// QQ 号
    /// </summary>
    public long UserId { get; init; }
    
    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; init; } = string.Empty;
    
    /// <summary>
    /// 性别
    /// </summary>
    public string Sex { get; init; } = string.Empty;

    /// <summary>
    /// QID
    /// </summary>
    public string Qid { get; init; } = string.Empty;
    
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; init; } = string.Empty;

    public object? GetExtra(string key)
    {
        return key switch
        {
            nameof(UserExtras.Id) => UserId,
            nameof(UserExtras.Nickname) => Nickname,
            _ => throw new Exception($"Extra \"{key}\" is not supported")
        };
    }

    static User()
    {
        ModuleManager.RegisterSerializableType<User>("milky-user");
    }
}