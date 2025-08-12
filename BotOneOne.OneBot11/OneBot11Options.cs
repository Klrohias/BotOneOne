namespace BotOneOne.OneBot11;

public struct OneBot11Options
{
    public TimeSpan InvocationTimeout { get; set; }

    public static OneBot11Options Default => new OneBot11Options
    {
        InvocationTimeout = TimeSpan.FromSeconds(30)
    };
}