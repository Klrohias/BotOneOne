using BotOneOne.OneBot11.Transfer.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotOneOne.OneBot11.Transfer.Packet;

public class MessageEventPacket : BaseEventPacket
{
    [JsonProperty("sender")] public UserDto Sender { get; set; } = new();

    [JsonProperty("message")] public JArray? Message { get; set; }

    [JsonProperty("group_id")] public long? GroupId { get; set; }
    
    [JsonProperty("sub_type")] public string? SubType { get; set; }
    [JsonProperty("message_type")] public string? MessageType { get; set; }

    [JsonProperty("message_id")] public long MessageId { get; set; }
}