namespace BotEleven.OneBot11;

/// <summary>
/// 针对 OneBot 11 协议的方言，需根据你使用的协议端选择
/// </summary>
public enum ProtocolDialect
{
    /// <summary>
    /// 标准的 OneBot 11 协议
    /// </summary>
    Standard,
    
    /// <summary>
    /// 【暂不支持】具有 NapCat 方言的 OneBot 11 协议 
    /// </summary>
    NapCat
}
