using BotOneOne.MessageFormat;

namespace BotOneOne;

public abstract class BotContext : IProtocol
{
    public event EventHandler<DirectMessageEventArgs>? DirectMessage;
    public event EventHandler<GroupMessageEventArgs>? GroupMessage;

    public abstract Task<MessageId> SendMessage(ChatId target, Message message);
    public abstract Task DeleteMessage(MessageId messageId);

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

public class DirectMessageEventArgs
{
    public ChatId User { get; set; }
    public Message Message { get; set; } = Message.Empty;
    public MessageId MessageId { get; set; }
    public DateTimeOffset Time { get; set; }
}

public class GroupMessageEventArgs
{
    public ChatId Group { get; set; }
    public ChatId User { get; set; }
    public Message Message { get; set; } = Message.Empty;
    public MessageId MessageId { get; set; }
    public DateTimeOffset Time { get; set; }
}
