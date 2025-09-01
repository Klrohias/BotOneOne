namespace BotEleven.Milky;

public class MilkyActionException(int code,
    string action, string message) : Exception($"Error occurred while invoking milky action \"{action}\": ({code}) {message}");