namespace BotOneOne.Standard;

public static class GroupExtras
{
    /// <summary>
    /// 群号
    /// </summary>
    public static readonly ExtraDefinition<long> Id = new(nameof(Id));
    
    /// <summary>
    /// 群聊名称
    /// </summary>
    public static readonly ExtraDefinition<string> Name = new(nameof(Name));
    
    /// <summary>
    /// 群人数
    /// </summary>
    public static readonly ExtraDefinition<int> MemberCount = new(nameof(MemberCount));
    
    /// <summary>
    /// 群容量
    /// </summary>
    public static readonly ExtraDefinition<int> Capacity = new(nameof(Capacity));
}