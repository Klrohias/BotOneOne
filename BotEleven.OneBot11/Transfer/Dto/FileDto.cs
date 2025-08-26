using Newtonsoft.Json;

namespace BotEleven.OneBot11.Transfer.Dto;

public class FileDto
{
    [JsonProperty("file")] public string File { get; set; }
}