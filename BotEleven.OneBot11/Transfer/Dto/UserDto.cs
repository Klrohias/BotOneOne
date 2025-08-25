using BotEleven.OneBot11.Entities;
using Newtonsoft.Json;

namespace BotEleven.OneBot11.Transfer.Dto;

public struct UserDto
{
    [JsonProperty("group_id")] public long? GroupId { get; set; }

    [JsonProperty("user_id")] public long UserId { get; set; }

    [JsonProperty("nickname")] public string? Nickname { get; set; }

    [JsonProperty("card")] public string? Card { get; set; }

    [JsonProperty("role")] public string? Role { get; set; }

    [JsonProperty("remark")] public string? Remark { get; set; }

    [JsonProperty("sex")] public string? Sex { get; set; }

    [JsonProperty("age")] public int? Age { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("level")] public int? Level { get; set; }

    [JsonProperty("join_time")] public long? JoinTime { get; set; }
    [JsonProperty("last_sent_time")] public long? LastSentTime { get; set; }
    
    public User ToUser()
    {
        return User.Of(
            UserId,
            new UserExtra(Name: Nickname,
                Age: Age,
                GroupMask: Card,
                Remark: Remark,
                Sex: Sex switch
                {
                    "female" => 0,
                    "male" => 1,
                    "unknown" => 2,
                    _ => 3
                },
                GroupHonor: Title,
                GroupLevel: Level,
                GroupRole: Role switch
                {
                    "owner" => 2,
                    "admin" => 1,
                    "member" => 0,
                    _ => 3
                },
                GroupJoinTime: JoinTime != null ? DateTimeOffset.FromUnixTimeSeconds(JoinTime.Value) : null,
                GroupLastSentTime: LastSentTime != null ? DateTimeOffset.FromUnixTimeSeconds(LastSentTime.Value) : null)
        );
    }
}