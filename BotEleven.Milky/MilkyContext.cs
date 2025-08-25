using BotEleven.MessageFormat;

namespace BotEleven.Milky;

public class MilkyContext(string serverEndpoint, MilkyOptions? options = null) : BaseMilkyContext(serverEndpoint, options)
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
}