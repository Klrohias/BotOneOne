using Newtonsoft.Json;

namespace BotOneOne.OneBot11.Transfer.Packet;

public class BaseIncomingPacket
{
    [JsonProperty("post_type")] public string? PostType { get; set; }
    [JsonIgnore] public bool IsEventPacket => PostType != null;
}