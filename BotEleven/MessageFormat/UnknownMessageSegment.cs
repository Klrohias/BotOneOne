using Newtonsoft.Json.Linq;

namespace BotEleven.MessageFormat;

public class UnknownMessageSegment : MessageSegment
{
    public string RawType { get; set; } = "unknown";
    public override string Type => RawType;
    public Dictionary<string, JToken> ExtensionData { get; set; } = [];

    public override string ToString()
    {
        return $"[Unknown type {RawType}]";
    }
}