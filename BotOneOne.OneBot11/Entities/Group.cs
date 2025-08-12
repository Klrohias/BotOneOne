namespace BotOneOne.OneBot11.Entities;

public struct Group
{
    public long Id;

    public static Group Of(long id)
    {
        return new Group { Id = id };
    }
}

public static partial class Extensions
{
    public static ChatId AsChatId(this Group group)
    {
        return new ChatId(group);
    }

    public static bool IsGroup(this ChatId chatId)
    {
        return chatId.Target is Group;
    }

    public static Group AsGroup(this ChatId chatId)
    {
        return chatId.IntoTransparent<Group>().Target;
    }
}
