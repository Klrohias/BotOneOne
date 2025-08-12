
using BotOneOne.MessageFormat;
using BotOneOne.OneBot11.Connectivity;
using BotOneOne.OneBot11.Entities;
using BotOneOne.OneBot11.Transfer;
using BotOneOne.OneBot11.Transfer.Dto;
using Newtonsoft.Json;

namespace BotOneOne.OneBot11;

public class OneBot11Context : BaseOneBot11Context
{
    public OneBot11Context(IConnectionSource connectionSource, OneBot11Options? options = null)
        : base(connectionSource, options)
    {
    }

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
        return InvokeAction("delete_msg", new MessageIdDto { MessageId = messageId.IntoTransparent<long>().Target });
    }
    
    protected override void HandleEvent(string eventType, string packet)
    {
        switch (eventType)
        {
            case "message":
            {
                var dto = JsonConvert.DeserializeObject<MessageEventDto>(packet)
                          ?? throw new Exception("Null packet deserialized");

                var message =
                    MessageSerializer.DeserializeMessage(
                        dto.Message ?? throw new Exception("Null message deserialized"));

                if (dto.MessageType == "group")
                {
                    // Group message
                    var groupId = dto.GroupId ?? throw new Exception("Null group deserialized");
                    var group = Group.Of(groupId).AsChatId();
                    var user = dto.Sender.ToUser().AsChatId();
                    var messageId = new MessageId(dto.MessageId);

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
                    var user = dto.Sender.ToUser().AsChatId();
                    var messageId = new MessageId(dto.MessageId);

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
