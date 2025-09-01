using BotEleven.MessageFormat;

namespace BotEleven;

/// <summary>
/// 机器人上下文，所有的操作、事件在此处理
/// </summary>
public abstract class BotContext
{
    /// <summary>
    /// 私聊消息事件，当收到私聊消息时被触发
    /// </summary>
    public event EventHandler<DirectMessageEventArgs>? OnDirectMessageReceived;

    /// <summary>
    /// 群聊消息事件，当收到群消息时被触发
    /// </summary>
    public event EventHandler<GroupMessageEventArgs>? OnGroupMessageReceived;

    /// <summary>
    /// 加群请求事件，当 Bot 作为群主或管理员时，若有加群请求则会被触发
    /// </summary>
    public event EventHandler<GroupRequestEventArgs>? OnGroupEntranceReceived;

    /// <summary>
    /// 邀请进群事件，当 Bot 被邀请进入某群时触发
    /// </summary>
    public event EventHandler<GroupRequestEventArgs>? OnGroupInvitationReceived;

    /// <summary>
    /// 添加好友事件，当有人向 Bot 发送好友请求时触发
    /// </summary>
    public event EventHandler<FriendAddEventArgs>? OnFriendAddRequested;

    /// <summary>
    /// 发送聊天消息
    /// </summary>
    /// <param name="target">发送目标</param>
    /// <param name="message">消息</param>
    public abstract Task<MessageId> SendMessage(ChatId target, Message message);

    /// <summary>
    /// 撤回聊天消息
    /// </summary>
    /// <param name="messageId">消息 Id</param>
    public abstract Task DeleteMessage(MessageId messageId);

    /// <summary>
    /// 获取消息内容
    /// </summary>
    /// <param name="messageId">消息 Id</param>
    public abstract Task<MessageDetail> GetMessage(MessageId messageId);

    /// <summary>
    /// 从 FileId 获取图片的真实的文件路径
    /// </summary>
    /// <param name="fileId">事件给出的 FileId</param>
    /// <returns>真实的图片路径</returns>
    public abstract Task<string> GetImage(FileId fileId);
    
    /// <summary>
    /// 从 FileId 获取语音的真实的文件路径
    /// </summary>
    /// <param name="fileId">事件给出的 FileId</param>
    /// <param name="format">需要的语音格式，默认为 `amr`</param>
    /// <returns>真实的语音文件路径</returns>
    public abstract Task<string> GetRecord(FileId fileId, string format = "amr");


    /// <summary>
    /// 对申请进群请求/邀请进群请求给出反应
    /// </summary>
    /// <param name="feedbackId">请求事件中给出的 FeedbackId</param>
    /// <param name="approve">是否同意该请求</param>
    /// <param name="comment">当不同意该请求时，提供原因</param>
    public abstract Task FeedbackGroupRequest(FeedbackId feedbackId, bool approve = false, string? comment = null);

    /// <summary>
    /// 对添加好友请求给出反应
    /// </summary>
    /// <param name="feedbackId">请求事件中给出的 FeedbackId</param>
    /// <param name="approve">是否同意该请求</param>
    public abstract Task FeedbackFriendRequest(FeedbackId feedbackId, bool approve = false);

    /// <summary>
    /// 退出群聊
    /// </summary>
    /// <param name="group">群聊的 ChatId</param>
    public abstract Task LeaveGroup(ChatId group);

    /// <summary>
    /// 修改群名称
    /// </summary>
    /// <param name="group">群聊的 ChatId</param>
    /// <param name="newGroupName">新群名称</param>
    public abstract Task RenameGroup(ChatId group, string newGroupName);

    /// <summary>
    /// 禁言群员
    /// </summary>
    /// <param name="group">要操作的群聊 ChatId</param>
    /// <param name="user">要操作的群员 ChatId</param>
    /// <param name="time">禁言的时长，单位为秒</param>
    public abstract Task SetGroupMute(ChatId group, ChatId user, int time = 0);

    /// <summary>
    /// 设置群组全员禁言
    /// </summary>
    /// <param name="group">要操作的群聊 ChatId</param>
    /// <param name="enable">是否启用全员禁言</param>
    public abstract Task SetGroupMute(ChatId group, bool enable = false);

    /// <summary>
    /// 群组踢人
    /// </summary>
    /// <param name="group">要操作的群聊 ChatId</param>
    /// <param name="user">要操作的群员 ChatId</param>
    /// <param name="blacklisted">是否将用户拉入该群黑名单，无法再申请加群</param>
    public abstract Task GroupKick(ChatId group, ChatId user, bool blacklisted = false);

    /// <summary>
    /// 获取完整的用户信息
    /// </summary>
    /// <param name="input">要获取的用户 ChatId</param>
    public abstract Task<ChatId> GetUserInfo(ChatId input);

    /// <summary>
    /// 获取完整的群成员信息
    /// </summary>
    /// <param name="group">要操作的群聊 ChatId</param>
    /// <param name="user">要获取的群员 ChatId</param>
    public abstract Task<ChatId> GetGroupMemberInfo(ChatId group, ChatId user);

    /// <summary>
    /// 获取完整的群信息
    /// </summary>
    /// <param name="group">要获取的群聊 ChatId</param>
    public abstract Task<ChatId> GetGroupInfo(ChatId group);
    
    /// <summary>
    /// 获取好友列表
    /// </summary>
    public abstract Task<IEnumerable<ChatId>> ListFriends();

    /// <summary>
    /// 获取群列表
    /// </summary>
    public abstract Task<IEnumerable<ChatId>> ListGroups();
    
    /// <summary>
    /// 获取群成员列表
    /// </summary>
    /// <param name="group">要获取的群聊 ChatId</param>
    public abstract Task<IEnumerable<ChatId>> ListGroupMembers(ChatId group);

    public abstract bool IsOpened { get; }
    public abstract void Open();
    public abstract void Close();
    public abstract Task InvokeAction<T>(string actionName, T? parameters);
    public abstract Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters);

    protected virtual void RaiseDirectMessageReceived(DirectMessageEventArgs eventArgs)
    {
        OnDirectMessageReceived?.Invoke(this, eventArgs);
    }

    protected virtual void RaiseGroupMessageReceived(GroupMessageEventArgs eventArgs)
    {
        OnGroupMessageReceived?.Invoke(this, eventArgs);
    }

    protected virtual void RaiseGroupEntranceReceived(GroupRequestEventArgs eventArgs)
    {
        OnGroupEntranceReceived?.Invoke(this, eventArgs);
    }

    protected virtual void RaiseGroupInvitationReceived(GroupRequestEventArgs eventArgs)
    {
        OnGroupInvitationReceived?.Invoke(this, eventArgs);
    }

    protected virtual void RaiseFriendAddRequested(FriendAddEventArgs eventArgs)
    {
        OnFriendAddRequested?.Invoke(this, eventArgs);
    }
}