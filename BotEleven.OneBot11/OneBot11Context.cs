using BotEleven.MessageFormat;
using BotEleven.OneBot11.Connectivity;
using BotEleven.OneBot11.Entities;
using BotEleven.OneBot11.Transfer;
using BotEleven.OneBot11.Transfer.Dto;
using BotEleven.OneBot11.Transfer.Packet;
using Newtonsoft.Json.Linq;

namespace BotEleven.OneBot11;

/// <summary>
/// 支持 OneBot 11 协议的 BotContext
/// </summary>
/// <param name="connectionSource">OneBot 11 的连接源</param>
/// <param name="options">连接选项，默认为 <see cref="OneBot11Options.Default"/></param>
public class OneBot11Context(IConnectionSource connectionSource, OneBot11Options? options = null)
    : BaseOneBot11Context(connectionSource, options)
{
    public override async Task<MessageId> SendMessage(ChatId target, Message message)
    {
        var dto = new SendMessageRequestDto();
        if (target.IsUser())
        {
            dto.UserId = target.AsUser().Id;
        }
        else
        {
            dto.GroupId = target.AsGroup().Id;
        }

        dto.Message = MessageSerializer.SerializeMessage(message);

        var response = await InvokeAction<MessageIdDto, SendMessageRequestDto>("send_msg", dto);
        return new MessageId(response.MessageId);
    }

    public override Task DeleteMessage(MessageId messageId)
    {
        return InvokeAction("delete_msg", new MessageIdDto { MessageId = messageId.AsTyped<long>().Target });
    }

    public override async Task<MessageDetail> GetMessage(MessageId messageId)
    {
        var response = await InvokeAction<GetMessageResponseDto, MessageIdDto>("get_msg",
                           new MessageIdDto { MessageId = messageId.AsTyped<long>().Target }) ??
                       throw new Exception("Get null response");

        return new MessageDetail(DateTimeOffset.FromUnixTimeSeconds(response.Time), response.Sender.ToUser().AsChatId(),
            MessageSerializer.DeserializeMessage(response.Message));
    }
    
    /// <summary>
    /// 获取完整的用户信息
    /// </summary>
    public async Task<ChatId<User>> GetUserInfo(ChatId input)
    {
        var userId = input.AsUser().Id;
        var user = await InvokeAction<UserDto, object>("get_stranger_info",
            new { user_id = userId });

        return user.ToUser().AsChatId();
    }

    /// <summary>
    /// 获取完整的群成员信息
    /// </summary>
    public async Task<ChatId<User>> GetGroupMemberInfo(ChatId group, ChatId user)
    {
        var groupId = group.AsGroup().Id;
        var userId = user.AsUser().Id;
        var userInfo = await InvokeAction<UserDto, object>("get_group_member_info",
            new { group_id = groupId, user_id = userId });

        return userInfo.ToUser().AsChatId();
    }

    /// <summary>
    /// 获取完整的群信息
    /// </summary>
    public async Task<ChatId<Group>> GetGroupInfo(ChatId group)
    {
        var groupId = group.AsGroup().Id;
        var groupDto = await InvokeAction<GroupDto, object>("get_group_info",
            new { group_id = groupId });

        return groupDto.ToGroup().AsChatId();
    }

    /// <summary>
    /// 群组踢人
    /// </summary>
    public Task GroupKick(ChatId group, ChatId user, bool blacklisted = false)
    {
        var groupId = group.AsGroup().Id;
        var userId = user.AsUser().Id;
        return InvokeAction("reject_add_request",
            new { group_id = groupId, user_id = userId, reject_add_request = blacklisted });
    }

    /// <summary>
    /// 禁言群员
    /// </summary>
    public Task SetGroupMute(ChatId group, ChatId user, int time = 0)
    {
        var groupId = group.AsGroup().Id;
        var userId = user.AsUser().Id;
        return InvokeAction("set_group_ban",
            new { group_id = groupId, user_id = userId, duration = time });
    }

    /// <summary>
    /// 设置群组全员禁言
    /// </summary>
    public Task SetGroupMute(ChatId group, bool enable = false)
    {
        var groupId = group.AsGroup().Id;
        return InvokeAction("set_group_whole_ban",
            new { group_id = groupId, enable });
    }

    /// <summary>
    /// 设置群名片
    /// </summary>
    public Task SetGroupMask(ChatId group, ChatId user, string mask = "")
    {
        var groupId = group.AsGroup().Id;
        return InvokeAction("set_group_card",
            new { group_id = groupId, card = mask });
    }

    /// <summary>
    /// 获取群成员列表
    /// </summary>
    public async Task<IEnumerable<ChatId<User>>> ListGroupMembers(ChatId group)
    {
        var groupId = group.AsGroup().Id;
        var userList = await InvokeAction<List<UserDto>, object>("get_group_member_list",
            new { group_id = groupId }) ?? throw new NullReferenceException("Get null response data");

        return userList.Select(x => x.ToUser().AsChatId());
    }

    /// <summary>
    /// 获取好友列表
    /// </summary>
    public async Task<IEnumerable<ChatId<User>>> ListFriends()
    {
        var userList = await InvokeAction<List<UserDto>, object>("get_friend_list",
            new { }) ?? throw new NullReferenceException("Get null response data");

        return userList.Select(x => x.ToUser().AsChatId());
    }

    /// <summary>
    /// 获取群列表
    /// </summary>
    public async Task<IEnumerable<ChatId<Group>>> ListGroups()
    {
        var groupList = await InvokeAction<List<GroupDto>, object>("get_group_list",
            new { }) ?? throw new NullReferenceException("Get null response data");

        return groupList.Select(x => x.ToGroup().AsChatId());
    }

    /// <summary>
    /// 对申请进群请求/邀请进群请求给出反应
    /// </summary>
    /// <param name="feedbackId">请求事件中给出的 FeedbackId</param>
    /// <param name="approve">是否同意该请求</param>
    /// <param name="comment">当不同意该请求时，提供原因</param>
    public async Task FeedbackGroupRequest(FeedbackId feedbackId, bool approve = false, string? comment = null)
    {
        var groupRequest = feedbackId.AsTyped<GroupRequest>().Target;
        await InvokeAction<object, object>("set_group_add_request",
            new
            {
                flag = groupRequest.Flag,
                sub_type = groupRequest.IsInvite ? "invite" : "add",
                approve,
                reason = comment
            });
    }
    
    /// <summary>
    /// 对添加好友请求给出反应
    /// </summary>
    /// <param name="feedbackId">请求事件中给出的 FeedbackId</param>
    /// <param name="approve">是否同意该请求</param>
    public async Task FeedbackFriendRequest(FeedbackId feedbackId, bool approve = false)
    {
        var groupRequest = feedbackId.AsTyped<GroupRequest>().Target;
        await InvokeAction<object, object>("set_friend_add_request",
            new
            {
                flag = groupRequest.Flag,
                approve,
            });
    }

    /// <summary>
    /// 退出群聊
    /// </summary>
    /// <param name="group">群聊的 ChatId</param>
    public async Task LeaveGroup(ChatId group)
    {
        if (!group.IsGroup())
        {
            throw new ArgumentException("The given chatId is not a group", nameof(group));
        }
        
        await InvokeAction<object, object>("set_group_leave",
            new
            {
                group_id = group.AsGroup().Id,
                is_dismiss = true,
            });
    }

    /// <summary>
    /// 修改群名称
    /// </summary>
    /// <param name="group">群聊的 ChatId</param>
    /// <param name="newGroupName">新群名称</param>
    public async Task RenameGroup(ChatId group, string newGroupName)
    {
        if (!group.IsGroup())
        {
            throw new ArgumentException("The given chatId is not a group", nameof(group));
        }
        
        await InvokeAction<object, object>("set_group_name",
            new
            {
                group_id = group.AsGroup().Id,
                group_name = newGroupName,
            });
    }
    
    /// <summary>
    /// 从 FileId 获取图片的真实的文件路径
    /// </summary>
    /// <param name="fileId">事件给出的 FileId</param>
    /// <returns>真实的图片路径</returns>
    public async Task<string> GetImage(FileId fileId)
    {
        var stringFileId = fileId.AsTyped<FileIdContent>().Target.File;
        var result = await InvokeAction<FileDto, FileDto>("get_image",
            new FileDto
            {
                File = stringFileId
            }) ?? throw new Exception("Got null response");

        return result.File;
    }

    /// <summary>
    /// 从 FileId 获取语音的真实的文件路径
    /// </summary>
    /// <param name="fileId">事件给出的 FileId</param>
    /// <param name="format">需要的语音格式，默认为 `amr`</param>
    /// <returns>真实的语音文件路径</returns>
    public async Task<string> GetRecord(FileId fileId, string format = "amr")
    {
        var stringFileId = fileId.AsTyped<FileIdContent>().Target.File;
        var result = await InvokeAction<FileDto, object>("get_record",
            new
            {
                file = stringFileId,
                out_format = format
            }) ?? throw new Exception("Got null response");

        return result.File;
    }

    protected override void HandleEvent(string eventType, JToken packet)
    {
        // check the event type and dispatch to handlers
        switch (eventType)
        {
            case "message":
                HandleMessageEvent(packet);
                break;

            case "request":
                HandleRequestEvent(packet);
                return;
            
            default:
                throw new Exception($"Unknown eventType \"{eventType}\"");
        }
    }

    private void HandleMessageEvent(JToken rawPacket)
    {
        var packet = rawPacket.ToObject<MessageEventPacket>() ?? throw new Exception("Null packet deserialized");

        var message =
            MessageSerializer.DeserializeMessage(
                packet.Message ?? throw new Exception("Null message deserialized"));
        var messageId = new MessageId(packet.MessageId);
        var user = packet.Sender.ToUser().AsChatId();

        if (packet.MessageType == "group")
        {
            // Group message
            var groupId = packet.GroupId ?? throw new Exception("Null group deserialized");
            var group = Group.Of(groupId).AsChatId();

            RaiseGroupMessageReceived(new GroupMessageEventArgs
            {
                Group = group,
                User = user,
                MessageId = messageId,
                Time = DateTimeOffset.FromUnixTimeSeconds(packet.Time),
                Message = message
            });
        }
        else
        {
            // Direct message
            RaiseDirectMessageReceived(new DirectMessageEventArgs
            {
                User = user,
                MessageId = messageId,
                Time = DateTimeOffset.FromUnixTimeSeconds(packet.Time),
                Message = message
            });
        }
    }

    private void HandleRequestEvent(JToken rawPacket)
    {
        var packet = rawPacket.ToObject<RequestEventPacket>() ?? throw new Exception("Null packet deserialized");
        switch (packet.RequestType)
        {
            case "group":
                switch (packet.SubType)
                {
                    case "add":
                        HandleGroupEntranceEvent(packet);
                        break;
                    case "invite":
                        HandleGroupInvitationEvent(packet);
                        break;
                    default:
                        throw new Exception($"Unknown subType \"{packet.SubType}\"");
                }

                break;
            
            case "friend":
                HandleFriendAddEvent(packet);
                break;

            default:
                throw new Exception($"Unknown requestType \"{packet.RequestType}\"");
        }
    }

    private void HandleGroupEntranceEvent(RequestEventPacket packet)
    {
        var groupId = packet.GroupId ?? throw new Exception("Null group deserialized");
        var group = Group.Of(groupId).AsChatId();
        
        var userId = packet.UserId ?? throw new Exception("Null user deserialized");
        var user = User.Of(userId).AsChatId();

        var request = new GroupRequest(false, packet.Flag);

        RaiseGroupEntranceReceived(new GroupRequestEventArgs
        {
            Group = group,
            User = user,
            Comment = packet.Comment,
            FeedbackId = new FeedbackId(request),
            Time = DateTimeOffset.FromUnixTimeSeconds(packet.Time)
        });
    }
    
    private void HandleGroupInvitationEvent(RequestEventPacket packet)
    {
        var groupId = packet.GroupId ?? throw new Exception("Null group deserialized");
        var group = Group.Of(groupId).AsChatId();
        
        var userId = packet.UserId ?? throw new Exception("Null user deserialized");
        var user = User.Of(userId).AsChatId();

        var request = new GroupRequest(true, packet.Flag);
        
        RaiseGroupInvitationReceived(new GroupRequestEventArgs
        {
            Group = group,
            User = user,
            FeedbackId = new FeedbackId(request),
            Time = DateTimeOffset.FromUnixTimeSeconds(packet.Time)
        });
    }
    
    private void HandleFriendAddEvent(RequestEventPacket packet)
    {
        var userId = packet.UserId ?? throw new Exception("Null user deserialized");
        var user = User.Of(userId).AsChatId();

        RaiseFriendAddRequested(new FriendAddEventArgs
        {
            User = user,
            Comment = packet.Comment,
            FeedbackId = new FeedbackId(packet.Flag),
            Time = DateTimeOffset.FromUnixTimeSeconds(packet.Time)
        });
    }
}

