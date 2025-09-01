namespace BotEleven.MessageFormat;

public class JsonMessageSegment(string serializedJson) : MessageSegment
{
    public override string Type => "json";
    public string SerializedJson { get; set; } = serializedJson;
}