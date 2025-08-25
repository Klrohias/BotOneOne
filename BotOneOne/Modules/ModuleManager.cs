using BotOneOne.Internals;

namespace BotOneOne.Modules;

public static class ModuleManager
{
    private static readonly BiDictionary<string, Type> SerializableTypes = new();

    static ModuleManager()
    {
        // built-in
        RegisterSerializableType<long>("i64");
        RegisterSerializableType<ulong>("u64");
        RegisterSerializableType<string>("string");
    }
    
    public static void RegisterSerializableType<T>(string id)
    {
        SerializableTypes.Add(id, typeof(T));
    }

    public static Type? GetSerializableTypeById(string id)
    {
        return SerializableTypes.GetByFirst(id);
    }
    
    public static string? GetIdBySerializableType(Type type)
    {
        return SerializableTypes.GetBySecond(type);
    }
}
