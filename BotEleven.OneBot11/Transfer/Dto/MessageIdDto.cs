using Newtonsoft.Json;

namespace BotEleven.OneBot11.Transfer.Dto;

public struct MessageIdDto
{
    [JsonProperty("message_id")] public long MessageId { get; set; }
}