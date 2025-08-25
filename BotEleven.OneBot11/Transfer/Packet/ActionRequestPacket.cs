using Newtonsoft.Json;

namespace BotEleven.OneBot11.Transfer.Packet;

public class ActionRequestPacket
{
    [JsonProperty("action")] public string Action { get; set; } = string.Empty;
    [JsonProperty("echo")] public string? Echo { get; set; }
}

public class ActionRequestPacket<T> : ActionRequestPacket
{
    [JsonProperty("params")] public T? Params { get; set; }
}