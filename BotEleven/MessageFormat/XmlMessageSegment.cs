namespace BotEleven.MessageFormat;

public class XmlMessageSegment(string serializedXml) : MessageSegment
{
    public override string Type => "xml";
    public string SerializedXml { get; set; } = serializedXml;
}