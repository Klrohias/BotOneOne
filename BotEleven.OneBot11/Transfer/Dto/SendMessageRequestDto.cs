using BotEleven.MessageFormat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotEleven.OneBot11.Transfer.Dto;

public struct SendMessageRequestDto
{
    [JsonProperty("user_id")] public long? UserId { get; set; }
    [JsonProperty("group_id")] public long? GroupId { get; set; }
    [JsonProperty("message")] public JToken Message { get; set; }
}