using Newtonsoft.Json;

namespace BotOneOne.OneBot11.Transfer.Packet;

public class RequestEventPacket : BaseEventPacket
{
    // From https://github.com/botuniverse/onebot-11/blob/master/event/request.md#%E8%AF%B7%E6%B1%82%E4%BA%8B%E4%BB%B6

    /*
字段名	数据类型	可能的值	说明
request_type	string	group	请求类型
sub_type	string	add、invite	请求子类型，分别表示加群请求、邀请登录号入群
group_id	number (int64)	-	群号
user_id	number (int64)	-	发送请求的 QQ 号
comment	string	-	验证信息
flag	string	-	请求 flag，在调用处理请求的 API 时需要传入
     */
    
    /*
字段名	数据类型	可能的值	说明
request_type	string	friend	请求类型
user_id	number (int64)	-	发送请求的 QQ 号
comment	string	-	验证信息
flag	string	-	请求 flag，在调用处理请求的 API 时需要传入
     */
    
    [JsonProperty("request_type")] public string RequestType { get; set; } = string.Empty;
    [JsonProperty("sub_type")] public string SubType { get; set; } = string.Empty;
    [JsonProperty("group_id")] public long? GroupId { get; set; }
    [JsonProperty("user_id")] public long? UserId { get; set; }
    [JsonProperty("comment")] public string Comment { get; set; } = string.Empty;
    [JsonProperty("flag")] public string Flag { get; set; } = string.Empty;
}