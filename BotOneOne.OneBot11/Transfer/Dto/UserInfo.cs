using BotOneOne.OneBot11.Entities;
using Newtonsoft.Json;

namespace BotOneOne.OneBot11.Transfer.Dto;

public class UserInfo
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

    public User ToUser()
    {
        return User.Of(
            id: UserId,
            name: Nickname,
            age: Age,
            groupMask: Card,
            remark: Remark,
            sex: Sex,
            groupHonor: Title,
            groupLevel: Level,
            groupRole: Role switch
            {
                "owner" => 2,
                "admin" => 1,
                "member" => 0,
                _ => 3
            }
        );
    }
}