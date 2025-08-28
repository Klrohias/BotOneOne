namespace BotEleven.MessageFormat;

public class ImageMessageSegment(FileId file) : MessageSegment
{
    public override string Type => "image";
    public FileId File { get; set; } = file;

    public override string ToString()
    {
        return $"[Image {File}]";
    }
}