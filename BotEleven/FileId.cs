using System.Runtime.Serialization;
using BotEleven.Internals;

namespace BotEleven;

/// <summary>
/// 文件 Id，涉及到文件操作时使用文件 Id 代表文件，需通过对应实现的相关方法获取真实的文件路径 / URL
/// </summary>
[Serializable]
public readonly struct FileId(object target) : ISerializable
{
    internal FileId(SerializationInfo info, StreamingContext context)
        : this(SerializeHelper.Deserialize(info) ?? throw new Exception("Deserialize null fileId"))
    {
    }
    
    public object Target { get; } = target;
    
    public FileId<T> AsTyped<T>() where T : notnull
    {
        return new FileId<T>((T)Target);
    }
    
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        SerializeHelper.Serialize(Target, info);
    }

    public static FileId FromPath(string path)
    {
        return new FileId(path);
    }
}


/// <summary>
/// 文件 Id，涉及到文件操作时使用文件 Id 代表文件，需通过对应实现的相关方法获取真实的文件路径 / URL
/// </summary>
[Serializable]
public readonly struct FileId<T>(T target)
    where T : notnull
{
    internal FileId(SerializationInfo info, StreamingContext context)
        : this(SerializeHelper.Deserialize<T>(info) ?? throw new Exception("Deserialize null fileId"))
    {
    }
    
    public T Target { get; } = target;

    public static implicit operator FileId(FileId<T> @this)
    {
        return new FileId(@this.Target);
    }
    
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        SerializeHelper.Serialize(Target, info);
    }
}