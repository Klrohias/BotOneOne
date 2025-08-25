using System.Runtime.Serialization;
using BotEleven.Internals;

namespace BotEleven;

/// <summary>
/// 代表一条消息的ID
/// </summary>
[Serializable]
public readonly struct MessageId(object target) : ISerializable
{
    internal MessageId(SerializationInfo info, StreamingContext context)
        : this(SerializeHelper.Deserialize(info) ?? throw new Exception("Deserialize null messageId"))
    {
    }

    public object Target { get; } = target;

    public MessageId<T> AsTyped<T>() where T : notnull
    {
        return new MessageId<T>((T)Target);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        SerializeHelper.Serialize(Target, info);
    }
}

/// <summary>
/// 代表一条消息的ID
/// </summary>
[Serializable]
public readonly struct MessageId<T>(T target) : ISerializable
    where T : notnull
{
    internal MessageId(SerializationInfo info, StreamingContext context)
        : this(SerializeHelper.Deserialize<T>(info) ?? throw new Exception("Deserialize null messageId"))
    {
    }
    
    public T Target { get; } = target;

    public static implicit operator MessageId(MessageId<T> @this)
    {
        return new MessageId(@this.Target);
    }
    
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        SerializeHelper.Serialize(Target, info);
    }
}
