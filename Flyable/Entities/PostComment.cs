using System.ComponentModel.DataAnnotations.Schema;
using Flyable.Entities;

namespace Flyable.Entities;

public class PostComment
{
    // 评论ID
    public int PostCommentId { get; set; }

    // 评论者ID
    public int UserId { get; set; }

    // 评论内容
    public string? Content { get; set; }

    // 评论发布时间
    public DateTime PublishTime { get; set; }

    // 评论被点赞数
    [NotMapped]
    public int LikeCount => LikeUsers.Count;

    // 评论被举报数
    public int ReportCount { get; set; }

    /// 评论状态
    /// 0:正常 1:被管理员软删除 无法再看见,除非用户申诉 2:被编辑锁定 3 被作者隐藏
    public int CommentStatus { get; set; }

    // 评论被回复数
    [NotMapped]
    public int ReplyCount => Replies?.Count ?? 0;

    public Post BelongsPost { get; set; } = null!;

    public List<Reply>? Replies { get; set; }

    // 评论是否被作者置顶
    public bool IsTop { get; set; }

    // 评论是否被作者推荐
    public bool IsRecommend { get; set; }

    // 评论是否为热评
    public bool IsHot { get; set; }

    /// <summary>
    /// 喜欢这条评论的用户
    /// </summary>
    public List<User> LikeUsers { get; set; } = new();

    public bool IsEdited { get; set; } = false;
}