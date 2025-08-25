namespace BotEleven.OneBot11.Entities;

public static class Extensions
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