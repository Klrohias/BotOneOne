using BotEleven.Modules;

namespace BotEleven.Milky.Entities;

// https://milky.ntqqrev.org/struct/GroupEntity
/*
字段名	类型	描述
group_id	int64	群号
group_name	string	群名称
member_count	int32	群成员数量
max_member_count	int32	群容量
 */

/// <summary>
/// 群实体
/// </summary>
public class Group
{
    /// <summary>
    /// 群号
    /// </summary>
    public long GroupId { get; init; }
    
    /// <summary>
    /// 群名称
    /// </summary>
    public string GroupName { get; init; } = string.Empty;
    
    /// <summary>
    /// 群成员数量
    /// </summary>
    public int MemberCount { get; init; }
    
    /// <summary>
    /// 群容量
    /// </summary>
    public int MaxMemberCount { get; init; }
    
    static Group()
    {
        ModuleManager.RegisterSerializableType<Group>("milky-group");
    }
}