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
                return new ImageMessageSegment(new FileId(new FileIdContent(target)));
            }
            case "record":
            {
                var target = segment["data"]?["file"]?.ToString()
                             ?? throw new NullReferenceException("Malformed recording segment");
                return new RecordingMessageSegment(new FileId(new FileIdContent(target)));
            }
            case "video":
            {
                var target = segment["data"]?["file"]?.ToString()
                             ?? throw new NullReferenceException("Malformed video segment");
                return new VideoMessageSegment(new FileId(new FileIdContent(target)));
            }
            case "contact":
            {
                var contactType = segment["data"]?["type"]?.ToString();
                
                var idString = segment["data"]?["id"]?.ToString()
                                  ?? throw new NullReferenceException("Invalid contact target");

                return new ContactMessageSegment(contactType switch
                {
                    "qq" => User.Of(long.Parse(idString)).AsChatId(),
                    "group" => Group.Of(long.Parse(idString)).AsChatId(),
                    _ => throw new Exception("Invalid contact type"),
                });
            }
            case "forward":
            {
                var idString = segment["data"]?["id"]?.ToString()
                               ?? throw new NullReferenceException("Invalid forward content");
                return new ForwardedMessageSegment(new MessageId(idString));
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

    private static string ExtractFileId(FileId fileId)
    {
        return fileId.Target switch
        {
            string stringFileId => stringFileId,
            FileIdContent fileIdContent => fileIdContent.File,
            _ => throw new ArgumentException("Unsupported fileId", nameof(fileId))
        };
    }

    private static JObject SerializeSegment(MessageSegment segment)
    {
        var result = new JObject();
        switch (segment)
        {
            case TextMessageSegment text:
                result["type"] = "text";
                result["data"] = new JObject
                {
                    ["text"] = text.Text
                };
                break;
            
            case AtMessageSegment at:
                if (!at.Target.IsUser())
                {
                    throw new Exception("Unexpected at target");    
                }
                
                result["type"] = "at";
                result["data"] = new JObject
                {
                    ["qq"] = at.Target.AsUser().Id
                };
                break;
            
            case ReplyMessageSegment reply:
                result["type"] = "reply";
                result["data"] = new JObject
                {
                    ["id"] = reply.MessageId.AsTyped<long>().Target
                };
                break;
            
            case ImageMessageSegment image:
                result["type"] = "image";
                result["data"] = new JObject
                {
                    ["file"] = ExtractFileId(image.File)
                };
                break;
            
            case RecordingMessageSegment recording:
                result["type"] = "record";
                result["data"] = new JObject
                {
                    ["file"] = ExtractFileId(recording.File)
                };
                break;
            
            case VideoMessageSegment video:
                result["type"] = "video";
                result["data"] = new JObject
                {
                    ["file"] = ExtractFileId(video.File)
                };
                break;
            
            case ContactMessageSegment contact:
                result["type"] = "contact";
                result["data"] = new JObject
                {
                    ["type"] = contact.Target.IsUser() ? "qq" : "group",
                    ["id"] = (contact.Target.IsUser()
                        ? contact.Target.AsUser().Id
                        : contact.Target.AsGroup().Id).ToString()
                };
                break;
            
            case ForwardedMessageSegment forwarded:
                throw new NotSupportedException("Serialize a forwarded message is not supported");

            default:
                throw new Exception("Unexpected unknown segment when parsing message");
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