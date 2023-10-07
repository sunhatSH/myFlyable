using System.ComponentModel.DataAnnotations.Schema;

namespace Flyable.Repositories.Entities;

/// <summary>
/// 为简单起见,这里的Reply不包含评论,只包含回复，只有评论才有回复，
/// 回复无法再被回复
/// </summary>
public class Reply
{
    public int ReplyId { get; set; }
    public int UserId { get; set; }
    public int ReplyTo { get; set; }

    /// <summary>
    /// 被回复的类型
    /// </summary>
    public int ReplyType { get; set; }

    public string Content { get; set; } = null!;
    public DateTime PublishTime { get; init; } = DateTime.Now;

    [NotMapped]
    public int LikeCount => LikeUsers?.Count ?? 0;
    public int ReportCount { get; set; }
    public int Status { get; set; }

    // ReSharper disable once CollectionNeverUpdated.Global
    public List<User>? LikeUsers { get; set; } = new();
}