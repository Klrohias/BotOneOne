namespace BotEleven.OneBot11;

public class OneBot11ActionException(int code) : Exception($"Error occurred while invoking OneBot11 action: {code}");