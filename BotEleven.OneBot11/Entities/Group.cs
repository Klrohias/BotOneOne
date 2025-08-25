using BotEleven.Modules;
using BotEleven.Standard;

namespace BotEleven.OneBot11.Entities;

/// <summary>
/// 代表一个群的对象
/// </summary>
/// <param name="Id">群号</param>
/// <param name="Extra">群聊的额外信息</param>
public record Group(long Id, GroupExtra? Extra) : IExtra
{
    static Group()
    {
        ModuleManager.RegisterSerializableType<Group>("onebot11-group");    
    }
    
    /// <summary>
    /// 构造一个 Group 对象，与 new 基本无异
    /// </summary>
    public static Group Of(long id, GroupExtra? extra = null)
    {
        return new Group(id, extra);
    }

    public object? GetExtra(string key)
    {
        if (key == nameof(GroupExtras.Id))
        {
            return Id;
        }
        
        if (Extra is null) throw new NotSupportedException("Not supported on this object");
        return key switch
        {
            nameof(GroupExtras.Capacity) => Extra.Capacity,
            nameof(GroupExtras.Name) => Extra.Name,
            nameof(GroupExtras.MemberCount) => Extra.MemberCount,
            _ => throw new NotSupportedException($"Extra \"{key}\" is not supported")
        };
    }
}

/// <summary>
/// 群聊的额外信息
/// </summary>
/// <param name="Name">群名称</param>
/// <param name="MemberCount">群人数</param>
/// <param name="Capacity">群容量</param>
public record GroupExtra(
    string? Name = null,
    int? MemberCount = null,
    int? Capacity = null);
    