using BotEleven.MessageFormat;
using BotEleven.Milky.Entities;

namespace BotEleven.Milky;

public class MilkyContext(string serverEndpoint, MilkyOptions? options = null) : BaseMilkyContext(serverEndpoint, options)
{
    public override Task<MessageId> SendMessage(ChatId target, Message message)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteMessage(MessageId messageId)
    {
        var content = messageId.AsTyped<MessageIdContent>().Target;
        if (content.IsGroup)
        {
            // group
            return InvokeAction("/recall_group_message", new
            {
                group_id = content.Peer,
                message_seq = content.SequenceId
            });
        }

        // direct message
        return InvokeAction("/recall_private_message", new
        {
            user_id = content.Peer,
            message_seq = content.SequenceId
        });
    }

    public override Task<MessageDetail> GetMessage(MessageId messageId)
    {
        throw new NotImplementedException();
    }
}