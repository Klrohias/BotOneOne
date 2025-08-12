using Newtonsoft.Json;

namespace BotOneOne.OneBot11.Transfer.Dto;

public struct GroupIdDto
{
    [JsonProperty("group_id")] public long GroupId { get; set; }
}