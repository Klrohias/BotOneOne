using System.Globalization;

namespace BotEleven.Milky.Internals;

internal static class Logger
{
    public static void LogException(Exception e)
    {
        if (AppContext.TryGetSwitch("Milky_LogEnabled", out var enabled) && enabled)
        {
            Console.WriteLine(
                $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - BotEleven.Milky [E] {e}");
        }
    }
}