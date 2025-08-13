using BotOneOne.MessageFormat;

namespace BotOneOne;

public abstract class BotContext
{
    public event EventHandler<DirectMessageEventArgs>? DirectMessage;
    public event EventHandler<GroupMessageEventArgs>? GroupMessage;

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

    protected virtual void RaiseDirectMessage(DirectMessageEventArgs eventArgs)
    {
        DirectMessage?.Invoke(this, eventArgs);
    }

    protected virtual void RaiseGroupMessage(GroupMessageEventArgs eventArgs)
    {
        GroupMessage?.Invoke(this, eventArgs);
    }
}