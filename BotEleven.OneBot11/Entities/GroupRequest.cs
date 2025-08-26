using BotEleven.Modules;

namespace BotEleven.OneBot11.Entities;

/// <summary>
/// 加群/邀请进群请求
/// </summary>
/// <param name="IsInvite">是否为邀请请求</param>
/// <param name="Flag">OneBot 11 的 Flag</param>
public record GroupRequest(
    bool IsInvite,
    string Flag)
{
    static GroupRequest()
    {
        ModuleManager.RegisterSerializableType<Group>("onebot11-group-request");    
    }
}