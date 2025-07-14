using System.ComponentModel.DataAnnotations.Schema;
using Flyable.Entities;

namespace Flyable.Entities;

/// <summary>
///     小说首页评论
/// </summary>
public class NovelComment
{
    // 评论ID
    public int NovelCommentId { get; set; }

    // 评论者ID
    public int UserId { get; set; }

    // 评论内容
    public string? Content { get; set; }

    // 评论发布时间
    public DateTime PublishTime { get; set; }

    // 所属小说ID
    public int NovelId { get; set; }

    // 该评论所属的小说
    public Novel? BelongsNovel { get; set; }

    // 评论被点赞数
    [NotMapped]
    public int LikeCount => LikeUsers.Count;

    // 评论被举报数
    [NotMapped]
    public int ReportCount => ReportUsers.Count;

    public List<User> ReportUsers { get; set; } = new();

    // 评论状态
    // 0:正常 1:被管理员软删除 无法再看见,除非用户申诉 2:被编辑锁定 3 被作者隐藏
    public int CommentStatus { get; set; }

    // 评论被回复数
    public int ReplyCount { get; set; }

    // 评论是否被作者置顶
    public bool IsTop { get; set; }

    // 评论是否被作者推荐
    public bool IsRecommend { get; set; }

    // 评论是否为热评
    public bool IsHot { get; set; }

    // 评论是否被编辑
    public bool IsEdited { get; set; } = false;

    public List<User> LikeUsers { get; set; } = new();
}