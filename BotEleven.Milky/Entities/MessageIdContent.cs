using BotEleven.Modules;

namespace BotEleven.Milky.Entities;

public record MessageIdContent(bool IsGroup,
    long Peer,
    long SequenceId)
{
    static MessageIdContent()
    {
        ModuleManager.RegisterSerializableType<MessageIdContent>("milky-message-id");
    }
}
