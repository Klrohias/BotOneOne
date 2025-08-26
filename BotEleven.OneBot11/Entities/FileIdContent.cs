namespace BotEleven.OneBot11.Entities;

public readonly struct FileIdContent(string file)
{
    public string File { get; } = file;
}