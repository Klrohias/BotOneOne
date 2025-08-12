using Newtonsoft.Json;

namespace BotOneOne.OneBot11.Transfer.Dto;

public class ActionRequestDto
{
    [JsonProperty("action")] public string Action { get; set; } = "";
    [JsonProperty("echo")] public string? Echo { get; set; }
}

public class ActionRequestDto<T> : ActionRequestDto
{
    [JsonProperty("params")] public T? Params { get; set; }
}