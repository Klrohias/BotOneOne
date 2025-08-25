using System.Runtime.Serialization;
using BotEleven.Internals;

namespace BotEleven;

/// <summary>
/// 反馈 Id，当某一事件需要 Bot 做出响应时（例如同意或拒绝加群），传入此对象
/// </summary>
[Serializable]
public readonly struct FeedbackId(object target) : ISerializable
{
    internal FeedbackId(SerializationInfo info, StreamingContext context)
        : this(SerializeHelper.Deserialize(info) ?? throw new Exception("Deserialize null messageId"))
    {
    }
    
    public object Target { get; } = target;
    
    public FeedbackId<T> AsTyped<T>() where T : notnull
    {
        return new FeedbackId<T>((T)Target);
    }
    
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        SerializeHelper.Serialize(Target, info);
    }
}


/// <summary>
/// 反馈 Id，当某一事件需要 Bot 做出响应时（例如同意或拒绝加群），传入此对象
/// </summary>
[Serializable]
public readonly struct FeedbackId<T>(T target)
    where T : notnull
{
    internal FeedbackId(SerializationInfo info, StreamingContext context)
        : this(SerializeHelper.Deserialize<T>(info) ?? throw new Exception("Deserialize null messageId"))
    {
    }
    
    public T Target { get; } = target;

    public static implicit operator FeedbackId(FeedbackId<T> @this)
    {
        return new FeedbackId(@this.Target);
    }
    
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        SerializeHelper.Serialize(Target, info);
    }
}
