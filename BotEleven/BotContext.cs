using BotEleven.MessageFormat;

namespace BotEleven;

/// <summary>
/// 机器人上下文，所有的操作、事件在此处理
/// </summary>
public abstract class BotContext
{
    /// <summary>
    /// 私聊消息事件，当收到私聊消息时被触发
    /// </summary>
    public event EventHandler<DirectMessageEventArgs>? DirectMessageReceived;
    
    /// <summary>
    /// 群聊消息事件，当收到群消息时被触发
    /// </summary>
    public event EventHandler<GroupMessageEventArgs>? GroupMessageReceived;

    /// <summary>
    /// 加群请求事件，当 Bot 作为群主或管理员时，若有加群请求则会被触发
    /// </summary>
    public event EventHandler<GroupRequestEventArgs>? GroupEntranceReceived; 
    
    /// <summary>
    /// 邀请进群事件，当 Bot 被邀请进入某群时触发
    /// </summary>
    public event EventHandler<GroupRequestEventArgs>? GroupInvitationReceived; 
    
    /// <summary>
    /// 添加好友事件，当有人向 Bot 发送好友请求时触发
    /// </summary>
    public event EventHandler<FriendAddEventArgs>? FriendAddRequested; 

    /// <summary>
    /// 发送聊天消息
    /// </summary>
    /// <param name="target">发送目标</param>
    /// <param name="message">消息</param>
    public abstract Task<MessageId> SendMessage(ChatId target, Message message);
    
    /// <summary>
    /// 撤回聊天消息
    /// </summary>
    /// <param name="messageId">消息 Id</param>
    public abstract Task DeleteMessage(MessageId messageId);

    /// <summary>
    /// 获取消息内容
    /// </summary>
    /// <param name="messageId">消息 Id</param>
    public abstract Task<MessageDetail> GetMessage(MessageId messageId);
    
    public abstract bool IsOpened { get; }
    public abstract void Open();
    public abstract void Close();
    public abstract Task InvokeAction<T>(string actionName, T? parameters);
    public abstract Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters);

    protected virtual void RaiseDirectMessageReceived(DirectMessageEventArgs eventArgs)
    {
        DirectMessageReceived?.Invoke(this, eventArgs);
    }

    protected virtual void RaiseGroupMessageReceived(GroupMessageEventArgs eventArgs)
    {
        GroupMessageReceived?.Invoke(this, eventArgs);
    }
    
    protected virtual void RaiseGroupEntranceReceived(GroupRequestEventArgs eventArgs)
    {
        GroupEntranceReceived?.Invoke(this, eventArgs);
    }
    
    protected virtual void RaiseGroupInvitationReceived(GroupRequestEventArgs eventArgs)
    {
        GroupInvitationReceived?.Invoke(this, eventArgs);
    }
    
    protected virtual void RaiseFriendAddRequested(FriendAddEventArgs eventArgs)
    {
        FriendAddRequested?.Invoke(this, eventArgs);
    }
}