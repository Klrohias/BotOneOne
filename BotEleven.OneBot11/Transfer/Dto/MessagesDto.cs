using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotEleven.OneBot11.Transfer.Dto;

public class MessagesDto
{
    [JsonProperty("message")] public JArray Message { get; set; }
}