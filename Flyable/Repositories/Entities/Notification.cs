namespace Flyable.Repositories.Entities;

// 用户和管理员收到的通知，分为广播和私信
public class Notification
{
    // 通知的唯一标识符,使用int类型
    public int NotificationId { get; set; }

    /// <summary>
    ///     是否是广播
    /// </summary>
    public bool IsBroadcast { get; set; }

    // 通知者ID
    public int SenderId { get; set; }

    // 通知接收者ID，如果是广播则为-1
    public int ReceiverId { get; set; }

    // 通知内容
    public string? Content { get; set; }

    // 通知发布时间
    public DateTime PublishTime { get; set; }

    // 是否已读，对于广播来说，该字段无意义
    public bool IsRead { get; set; }
}