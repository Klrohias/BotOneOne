using BotEleven.OneBot11.Entities;
using Newtonsoft.Json;

namespace BotEleven.OneBot11.Transfer.Dto;

public struct GroupDto
{
    [JsonProperty("group_id")] public long GroupId { get; set; }
    [JsonProperty("group_name")] public string GroupName { get; set; }
    [JsonProperty("member_count")] public int MemberCount { get; set; }
    [JsonProperty("max_member_count")] public int MaxMemberCount { get; set; }

    public Group ToGroup()
    {
        return Group.Of(GroupId, new GroupExtra(Name: GroupName, MemberCount: MemberCount, Capacity: MaxMemberCount));
    }
}
