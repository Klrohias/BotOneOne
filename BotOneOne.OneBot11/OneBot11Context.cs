using BotOneOne.MessageFormat;
using BotOneOne.OneBot11.Connectivity;
using BotOneOne.OneBot11.Entities;
using BotOneOne.OneBot11.Transfer;
using BotOneOne.OneBot11.Transfer.Dto;
using BotOneOne.OneBot11.Transfer.Packet;
using Newtonsoft.Json;

namespace BotOneOne.OneBot11;

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
        var response = await InvokeAction<GetMessageResponseDto, MessageIdDto>("send_msg",
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
    
    protected override void HandleEvent(string eventType, string packet)
    {
        switch (eventType)
        {
            case "message":
            {
                var dto = JsonConvert.DeserializeObject<MessageEventPacket>(packet)
                          ?? throw new Exception("Null packet deserialized");

                var message =
                    MessageSerializer.DeserializeMessage(
                        dto.Message ?? throw new Exception("Null message deserialized"));
                var messageId = new MessageId(dto.MessageId);
                var user = dto.Sender.ToUser().AsChatId();
                
                if (dto.MessageType == "group")
                {
                    // Group message
                    var groupId = dto.GroupId ?? throw new Exception("Null group deserialized");
                    var group = Group.Of(groupId).AsChatId();

                    RaiseGroupMessage(new GroupMessageEventArgs
                    {
                        Group = group,
                        User = user,
                        MessageId = messageId,
                        Time = DateTimeOffset.FromUnixTimeSeconds(dto.Time),
                        Message = message
                    });
                }
                else
                {
                    // Direct message
                    RaiseDirectMessage(new DirectMessageEventArgs
                    {
                        User = user,
                        MessageId = messageId,
                        Time = DateTimeOffset.FromUnixTimeSeconds(dto.Time),
                        Message = message
                    });
                }

                break;
            }
            default:
                throw new Exception($"Unknown eventType \"{eventType}\"");
        }
    }
}
