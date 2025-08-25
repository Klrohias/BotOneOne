using Newtonsoft.Json;

namespace BotEleven.OneBot11.Transfer.Dto;

public struct GroupIdDto
{
    [JsonProperty("group_id")] public long GroupId { get; set; }
}