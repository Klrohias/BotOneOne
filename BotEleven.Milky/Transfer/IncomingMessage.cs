using Newtonsoft.Json;

namespace BotEleven.Milky.Transfer;

/*
字段名	类型	描述
message_scene	"friend"	表示好友消息
peer_id	int64	好友 QQ 号或群号
message_seq	int64	消息序列号
sender_id	int64	发送者 QQ 号
time	int64	消息 Unix 时间戳（秒）
segments	IncomingSegment[]	消息段列表
friend	FriendEntity	好友信息
 */

/*
字段名	类型	描述
message_scene	"group"	表示群消息
group	GroupEntity	群信息
group_member	GroupMemberEntity	群成员信息
 */

/*
message_scene	"temp"	表示临时会话消息 
 */

public class IncomingMessage
{
    [JsonProperty("message_scene")]
    public string MessageScene { get; set; } = "friend"; // 默认值设为"friend"

    [JsonProperty("peer_id")]
    public long PeerId { get; set; }

    [JsonProperty("message_seq")]
    public long MessageSeq { get; set; }

    [JsonProperty("sender_id")]
    public long SenderId { get; set; }

    [JsonProperty("time")]
    public long Time { get; set; } // Unix时间戳（秒）

    [JsonProperty("segments")]
    public List<IncomingSegment> Segments { get; set; }

    [JsonProperty("friend")]
    public FriendEntity? Friend { get; set; }
    
    [JsonProperty("group")]
    public GroupEntity? Group { get; set; }
    
    [JsonProperty("group_member")]
    public GroupMemberEntity? GroupMemberEntity { get; set; }
}