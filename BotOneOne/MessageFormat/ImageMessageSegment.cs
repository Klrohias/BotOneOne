namespace BotOneOne.MessageFormat;

public class ImageMessageSegment : MessageSegment<ImageMessageSegment.Payload>
{
    public ImageMessageSegment(string file)
    {
        Data = new Payload { File = file };
    }

    public override string ToString()
    {
        return $"[Image at {Data.File}]";
    }

    public struct Payload
    {
        public string File { get; set; }
    }

    public override string Type => "image";
}