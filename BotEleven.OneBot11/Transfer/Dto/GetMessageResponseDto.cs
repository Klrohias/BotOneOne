using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotEleven.OneBot11.Transfer.Dto;

public class GetMessageResponseDto
{
    [JsonProperty("time")] public long Time { get; set; }
    [JsonProperty("sender")] public UserDto Sender { get; set; }
    [JsonProperty("message")] public JArray Message { get; set; }
}