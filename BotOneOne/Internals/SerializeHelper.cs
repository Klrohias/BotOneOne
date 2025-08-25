using System.Runtime.Serialization;
using BotOneOne.Modules;

namespace BotOneOne.Internals;

internal static class SerializeHelper
{
    public static object? Deserialize(SerializationInfo info)
    {
        var id = (string?)info.GetValue("type", typeof(string)) ??
                 throw new Exception("The deserialization of this object is not supported");

        var type = ModuleManager.GetSerializableTypeById(id) ??
                   throw new Exception($"The deserialization of \"{id}\" is not supported");

        return info.GetValue("target", type);
    }

    public static T? Deserialize<T>(SerializationInfo info)
    {
        return (T?)Deserialize(info);
    }
    
    public static void Serialize(object target, SerializationInfo info)
    {
        var id = ModuleManager.GetIdBySerializableType(target.GetType())
                   ?? throw new Exception("This implementation of this type doesn't support serialize");
        
        info.AddValue("type", id);
        info.AddValue("target", target);
    }
}
