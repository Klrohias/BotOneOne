using BotEleven.MessageFormat;
using BotEleven.OneBot11.Entities;
using Newtonsoft.Json.Linq;

namespace BotEleven.OneBot11.Transfer;

public static class MessageSerializer
{
    private static readonly Dictionary<string, Type> SegmentTypes = new()
    {
        { "text", typeof(TextMessageSegment) },
        { "at", typeof(AtMessageSegment) },
        { "image", typeof(ImageMessageSegment) },
        { "reply", typeof(ReplyMessageSegment) }
    };

    private static MessageSegment DeserializeSegment(JToken segment)
    {
        var type = segment["type"]?.ToString();
        switch (type)
        {
            case "text":
            {
                return new TextMessageSegment(
                    segment["data"]?["text"]?.ToString() ?? throw new NullReferenceException("Malformed text segment"));
            }
            case "at":
            {
                var target = segment["data"]?["qq"]?.ToObject<long>() 
                             ?? throw new NullReferenceException("Malformed at segment");
                return new AtMessageSegment(User.Of(target).AsChatId());
            }
            case "reply":
            {
                var target = segment["data"]?["id"]?.ToObject<long>() 
                             ?? throw new NullReferenceException("Malformed reply segment");
                return new ReplyMessageSegment(new MessageId(target));
            }
            case "image":
            {
                var target = segment["data"]?["file"]?.ToString()
                             ?? throw new NullReferenceException("Malformed image segment");
                return new ImageMessageSegment(target);
            }
            default:
            {
                return new UnknownMessageSegment();
            }
        }
    }

    public static Message DeserializeMessage(JArray array)
    {
        var result = new Message();
        foreach (var item in array)
        {
            result.Append(DeserializeSegment(item));
        }

        return result;
    }

    private static JObject SerializeSegment(MessageSegment segment)
    {
        var result = new JObject();
        if (segment is UnknownMessageSegment)
        {
            throw new Exception("Unexpected unknown segment when parsing message");
        }

        if (segment is TextMessageSegment text)
        {
            result["type"] = text.Type;
            result["data"] = new JObject
            {
                ["text"] = text.Data.Text
            };
        }
        else if (segment is AtMessageSegment at)
        {
            if (!at.Data.Target.IsUser())
            {
                throw new Exception("Unexpected at target");
            }
            
            result["type"] = at.Type;
            result["data"] = new JObject
            {
                ["qq"] = at.Data.Target.AsUser().Id
            };
        } else if (segment is ReplyMessageSegment reply)
        {
            result["type"] = reply.Type;
            result["data"] = new JObject
            {
                ["id"] = reply.Data.MessageId.AsTyped<long>().Target
            };
        } else if (segment is ImageMessageSegment image)
        {
            result["type"] = image.Type;
            result["data"] = new JObject
            {
                ["file"] = image.Data.File
            };
        }

        return result;
    }

    public static JArray SerializeMessage(Message message)
    {
        var result = new JArray();

        foreach (var segment in message.Segments)
        {
           result.Add(SerializeSegment(segment));
        }
        
        return result;
    }
}