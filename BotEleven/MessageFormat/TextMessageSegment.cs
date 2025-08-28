namespace BotEleven.MessageFormat;

public class TextMessageSegment(string content) : MessageSegment
{
    public override string Type => "text";
    public string Text { get; set; } = content;

    public override string ToString()
    {
        return Text;
    }
}