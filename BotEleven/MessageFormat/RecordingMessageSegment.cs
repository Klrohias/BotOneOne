namespace BotEleven.MessageFormat;

public class RecordingMessageSegment(FileId file) : MessageSegment
{
    public override string Type => "recording";
    public FileId File { get; set; } = file;

    public override string ToString()
    {
        return $"[Recording {File}]";
    }
}
