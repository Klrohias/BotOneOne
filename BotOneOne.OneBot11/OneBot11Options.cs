namespace BotOneOne.OneBot11;

public struct OneBot11Options
{
    public TimeSpan InvocationTimeout { get; set; }

    public static OneBot11Options Default => new()
    {
        InvocationTimeout = TimeSpan.FromSeconds(30)
    };
}