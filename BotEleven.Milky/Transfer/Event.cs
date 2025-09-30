using Newtonsoft.Json;

namespace BotEleven.Milky.Transfer;

/*
字段名	类型	描述
event_type	string	类型区分字段
time	int64	事件 Unix 时间戳（秒）
self_id	int64	机器人 QQ 号
data	object	与 event_type 有关
 */

public class Event<T>
{
    [JsonProperty("event_type")] public string EventType { get; set; } = string.Empty;
    [JsonProperty("time")] public long Time { get; set; }
    [JsonProperty("self_id")] public long SelfId { get; set; }
    [JsonProperty("data")] public T? Data { get; set; }
}