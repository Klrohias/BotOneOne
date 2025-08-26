namespace BotEleven.MessageFormat;

public class VideoMessageSegment(FileId file) : MessageSegment
{
    public override string Type => "video";
    public FileId File { get; set; } = file;

    public override string ToString()
    {
        return $"[Recording {File}]";
    }
}
