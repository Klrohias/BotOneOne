using System.Runtime.Serialization;
using BotOneOne.Internals;

namespace BotOneOne;

/// <summary>
/// 代表一个「能往其中发送消息」的对象
/// </summary>
[Serializable]
public readonly struct ChatId(object target) : ISerializable
{
    internal ChatId(SerializationInfo info, StreamingContext context)
        : this(SerializeHelper.Deserialize(info) ?? throw new Exception("Deserialize null chatId"))
    {
    }

    public object Target { get; } = target;
    
    public ChatId<T> AsTyped<T>() where T : notnull
    {
        return new ChatId<T>((T)Target);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        SerializeHelper.Serialize(Target, info);
    }
}


/// <summary>
/// 代表一个「能往其中发送消息」的对象
/// </summary>
[Serializable]
public readonly struct ChatId<T>(T target) : ISerializable
    where T : notnull
{
    internal ChatId(SerializationInfo info, StreamingContext context)
        : this(SerializeHelper.Deserialize<T>(info) ?? throw new Exception("Deserialize null chatId"))
    {
    }
    
    public T Target { get; } = target;

    public static implicit operator ChatId(ChatId<T> @this)
    {
        return new ChatId(@this.Target);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        SerializeHelper.Serialize(Target, info);
    }
}
