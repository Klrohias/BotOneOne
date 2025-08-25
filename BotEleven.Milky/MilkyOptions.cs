namespace BotEleven.Milky;

public class MilkyOptions
{
    public string? Token { get; set; }
    public TimeSpan InvocationTimeout { get; set; }

    public static MilkyOptions Default => new()
    {
        InvocationTimeout = TimeSpan.FromSeconds(30)
    };
}
