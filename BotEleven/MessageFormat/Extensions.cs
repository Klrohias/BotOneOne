namespace BotEleven.MessageFormat;

public static class Extensions
{
    /// <summary>
    /// 拼接一个文本消息段
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="content">要拼接的文本</param>
    public static Message Text(this Message message, string content)
    {
        message.Append(new TextMessageSegment(content));
        return message;
    }

    /// <summary>
    /// 拼接一个 At（艾特） 消息段
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="target">要 At 的对象</param>
    public static Message At(this Message message, ChatId target)
    {
        message.Append(new AtMessageSegment(target));
        return message;
    }
    
    /// <summary>
    /// 拼接一个图片消息段
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="file">文件</param>
    public static Message Image(this Message message, FileId file)
    {
        message.Append(new ImageMessageSegment(file));
        return message;
    }
    
    /// <summary>
    /// 拼接一个音频 / 录音消息段
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="file">文件</param>
    public static Message Record(this Message message, FileId file)
    {
        message.Append(new RecordingMessageSegment(file));
        return message;
    }
    
    /// <summary>
    /// 拼接一个回复消息段
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="messageId">要回复消息的 Id</param>
    public static Message Reply(this Message message, MessageId messageId)
    {
        message.Append(new ReplyMessageSegment(messageId));
        return message;
    }
    
    /// <summary>
    /// 拼接一个视频消息段
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="file">要发送的视频的文件 Id</param>
    public static Message Video(this Message message, FileId file)
    {
        message.Append(new VideoMessageSegment(file));
        return message;
    }
    
    /// <summary>
    /// 拼接一个好友推荐 / 群聊推荐消息段
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="contact">要推荐的用户/群聊的 ChatId</param>
    public static Message Contact(this Message message, ChatId contact)
    {
        message.Append(new ContactMessageSegment(contact));
        return message;
    }
    
    /// <summary>
    /// 拼接一个 Json 消息段
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="serializedJson">Json 内容</param>
    public static Message Json(this Message message, string serializedJson)
    {
        message.Append(new JsonMessageSegment(serializedJson));
        return message;
    }
    
    /// <summary>
    /// 拼接一个 Xml 消息段
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="serializedXml">Xml 内容</param>
    public static Message Xml(this Message message, string serializedXml)
    {
        message.Append(new XmlMessageSegment(serializedXml));
        return message;
    }
}