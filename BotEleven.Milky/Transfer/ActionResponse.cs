using Newtonsoft.Json;

namespace BotEleven.Milky.Transfer;

public class ActionResponse<T>
{
    [JsonProperty("status")] public string Status { get; set; } = "ok";

    [JsonProperty("message")] public string? Message { get; set; }

    [JsonProperty("retcode")] public int RetCode { get; set; }

    [JsonProperty("data")] public T? Data { get; set; }
}

// From https://milky.ntqqrev.org/guide/communication

/*
    // 成功响应示例
    {
      "status": "ok",
      "retcode": 0, // 成功时的 retcode 为 0
      "data": {
        "message_seq": 23333,
       "time": 1234567890
      }
    }
 */
 
/*
    // 失败响应示例
    {
      "status": "failed",
      "retcode": -400, // 参数解析失败时，retcode 为 -400
      "message": "user_id (-1) 不是一个合法的 QQ 号"
    } 
*/