namespace BotEleven.OneBot11.Entities;

/// <summary>
/// OneBot 11 File的实现
/// </summary>
/// <param name="file">服务端上的 File</param>
public readonly struct FileIdContent(string file)
{
    public string File { get; } = file;
}