using BotEleven.MessageFormat;

namespace BotEleven.Milky;

public class MilkyContext : BotContext
{
    public override Task<MessageId> SendMessage(ChatId target, Message message)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteMessage(MessageId messageId)
    {
        throw new NotImplementedException();
    }

    public override Task<MessageDetail> GetMessage(MessageId messageId)
    {
        throw new NotImplementedException();
    }

    public override bool IsOpened { get; }
    public override void Open()
    {
        throw new NotImplementedException();
    }

    public override void Close()
    {
        throw new NotImplementedException();
    }

    public override Task InvokeAction<T>(string actionName, T? parameters) where T : default
    {
        throw new NotImplementedException();
    }

    public override Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters) where TReturn : default where TParam : default
    {
        throw new NotImplementedException();
    }
}