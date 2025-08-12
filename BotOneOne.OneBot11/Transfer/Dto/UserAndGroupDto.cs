using Newtonsoft.Json;

namespace BotOneOne.OneBot11.Transfer.Dto;

public struct UserAndGroupDto
{
    [JsonProperty("user_id")] public long UserId { get; set; }
    [JsonProperty("group_id")] public long GroupId { get; set; }
}