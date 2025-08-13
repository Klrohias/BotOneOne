namespace BotOneOne.OneBot11.Entities;

public readonly struct Group
{
    public readonly long Id;
    public readonly GroupExtra? Extra;

    private Group(long id, GroupExtra? extra)
    {
        Id = id;
        Extra = extra;
    }

    public static Group Of(long id, GroupExtra? extra = null)
    {
        return new Group(id, extra);
    }
}

/// <summary>
/// 群额外信息
/// </summary>
/// <param name="Name">群名称</param>
/// <param name="MemberCount">群人数</param>
/// <param name="Capacity">群容量</param>
public record GroupExtra(
    string Name,
    int MemberCount,
    int Capacity);

public static partial class Extensions
{
    public static ChatId<Group> AsChatId(this Group group)
    {
        return new ChatId<Group>(group);
    }

    public static bool IsGroup(this ChatId chatId)
    {
        return chatId.Target is Group;
    }

    public static Group AsGroup(this ChatId chatId)
    {
        return chatId.AsTyped<Group>().Target;
    }
}
