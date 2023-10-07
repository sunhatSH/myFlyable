namespace Flyable.Repositories.Entities;

//用户之间发送的消息
public class Message
{
    /// <summary>
    ///     消息的唯一标识符,使用int类型即可
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    ///     消息的内容
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    ///     消息的状态
    ///     0:未处理 会在一定时间延迟后存储数据库
    ///     1:已处理,为1的消息会在三天后删除
    ///     2:不予处理,为2的消息会返还给消息的发送者
    /// </summary>
    public int MessageStatus { get; set; }

    /// <summary>
    ///     消息的创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    ///     消息的目的对象UserId或广播Id（-1）
    /// </summary>
    public int MessageTarget { get; set; }

    /// <summary>
    ///     是否已读
    /// </summary>
    public bool IsRead { get; set; }


    /// <summary>
    ///     消息的发送者UserId
    /// </summary>
    public int SenderId { get; set; }
}