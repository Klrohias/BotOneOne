namespace BotOneOne.Standard;

public static partial class Extensions
{
    public static long GetId(this ChatId chatIdWithStandardSupports)
    {
        return chatIdWithStandardSupports.AsTyped<IExtra>().Target.GetExtra(UserExtras.Id);
    }
    
    public static string? GetNickname(this ChatId chatIdWithStandardSupports)
    {
        return chatIdWithStandardSupports.AsTyped<IExtra>().Target.GetExtra(UserExtras.Nickname);
    }
}