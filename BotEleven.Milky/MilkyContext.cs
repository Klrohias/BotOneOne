using BotEleven.MessageFormat;
using BotEleven.Milky.Entities;

namespace BotEleven.Milky;

public sealed class MilkyContext(string serverEndpoint, MilkyOptions? options = null) : BaseMilkyContext(serverEndpoint, options)
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

    public override Task<string> GetImage(FileId fileId)
    {
        throw new NotImplementedException();
    }

    public override Task<string> GetRecord(FileId fileId, string format = "amr")
    {
        throw new NotImplementedException();
    }

    public override Task FeedbackGroupRequest(FeedbackId feedbackId, bool approve = false, string? comment = null)
    {
        throw new NotImplementedException();
    }

    public override Task FeedbackFriendRequest(FeedbackId feedbackId, bool approve = false)
    {
        throw new NotImplementedException();
    }

    public override Task LeaveGroup(ChatId group)
    {
        throw new NotImplementedException();
    }

    public override Task RenameGroup(ChatId group, string newGroupName)
    {
        throw new NotImplementedException();
    }

    public override Task SetGroupMute(ChatId group, ChatId user, int time = 0)
    {
        throw new NotImplementedException();
    }

    public override Task SetGroupMute(ChatId group, bool enable = false)
    {
        throw new NotImplementedException();
    }

    public override Task GroupKick(ChatId group, ChatId user, bool blacklisted = false)
    {
        throw new NotImplementedException();
    }

    public override Task<ChatId> GetUserInfo(ChatId input)
    {
        throw new NotImplementedException();
    }

    public override Task<ChatId> GetGroupMemberInfo(ChatId group, ChatId user)
    {
        throw new NotImplementedException();
    }

    public override Task<ChatId> GetGroupInfo(ChatId group)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<ChatId>> ListFriends()
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<ChatId>> ListGroups()
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<ChatId>> ListGroupMembers(ChatId group)
    {
        throw new NotImplementedException();
    }
}