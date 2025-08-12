using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotOneOne.OneBot11.Transfer.Dto;

public class ActionResponseDto
{
    [JsonProperty("status")] public string Status { get; set; } = "failed";
    [JsonProperty("retcode")] public int ReturnCode { get; set; }
    [JsonProperty("echo")] public string? Echo { get; set; }
    [JsonExtensionData] public Dictionary<string, JToken> ExtensionData { get; set; } = [];
}