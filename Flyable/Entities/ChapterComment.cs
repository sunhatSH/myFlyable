using System.ComponentModel.DataAnnotations.Schema;
using Flyable.Entities;

namespace Flyable.Entities;

public class ChapterComment
{
    // 评论ID
    public int ChapterCommentId { get; set; }

    // 评论者ID
    public int UserId { get; set; }

    /// 评论内容
    public string? Content { get; set; }

    // 评论发布时间
    public DateTime PublishTime { get; set; }

    // 该评论所属的章节 导航属性
    public Chapter? BelongsChapter { get; set; }

    [NotMapped]
    // 评论被点赞数
    public int LikeCount => LikeUsers.Count;

    // 评论被举报数
    public int ReportCount { get; set; }

    /// 评论状态
    /// 0:正常 1:被管理员软删除 无法再看见,除非用户申诉 2:被编辑锁定 3 被作者隐藏
    public int CommentStatus { get; set; }

    // 评论被回复数
    [NotMapped]
    public int ReplyCount => Replies?.Count ?? 0;

    // 评论是否被作者置顶
    public bool IsTop { get; set; }

    // 评论是否被作者推荐
    public bool IsRecommend { get; set; }

    // 评论是否为热评
    public bool IsHot { get; set; }

    public List<User> LikeUsers { get; set; } = new();

    public List<Reply>? Replies { get; set; }
}