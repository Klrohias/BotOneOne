using System.Globalization;

namespace BotOneOne.OneBot11;

internal static class Utils
{
    public static void LogException(Exception e)
    {
        if (AppContext.TryGetSwitch("OneBot11_LogEnabled", out var enabled) && enabled)
        {
            Console.WriteLine(
                $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - BotOneOne.OneBot11 [E] {e}");
        }
    }
}
