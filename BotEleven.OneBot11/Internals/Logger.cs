using System.Globalization;

namespace BotEleven.OneBot11.Internals;

internal static class Logger
{
    public static void LogException(Exception e)
    {
        if (AppContext.TryGetSwitch("OneBot11_LogEnabled", out var enabled) && enabled)
        {
            Console.WriteLine(
                $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - BotEleven.OneBot11 [E] {e}");
        }
    }
}
