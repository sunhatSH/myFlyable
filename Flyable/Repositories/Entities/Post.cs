using Microsoft.EntityFrameworkCore;

namespace Flyable.Repositories.Entities;

[Index(nameof(PostId), nameof(UserId), nameof(LastAlterTime), nameof(PublishTime))]
public class Post
{
    // 帖子ID
    public int PostId { get; set; }

    // 帖子标题
    public string? Title { get; set; }

    // 帖子内容
    public string? Content { get; set; }

    // 帖子发布时间
    public DateTime PublishTime { get; set; }

    // 帖子最近修改时间
    public DateTime LastAlterTime { get; set; }

    // 帖子作者ID
    public int UserId { get; set; }

    // 帖子作者名
    public string? Username { get; set; }

    // 帖子被点赞数
    public int LikeCount { get; set; }

    // 帖子被举报数
    public int ReportCount { get; set; }

    // 帖子状态

    public int PostStatus { get; set; }

    // 帖子被回复数
    public int ReplyCount { get; set; }

    //帖子是否被编辑推荐
    public bool IsRecommend { get; set; }

    //帖子是否在首页显示
    public bool IsShowOnHomePage { get; set; }

    // 帖子是否为热帖
    public bool IsHot { get; set; }

    // 帖子的标签 以英文;分割 例如: "标签1;标签2;标签3"
    public List<PostTag>? PostTags { get; set; }

    //帖子总共获得的羽毛打赏
    public int TotalFeather { get; set; }

    //帖子总共获得的彩色石打赏
    public int TotalColorStone { get; set; }

    // 上次管理时间
    public DateTime LastModifyTime { get; set; }

    // 上一个提交修改帖子状态的管理员ID
    public int LastModifyAdminId { get; set; }

    // 帖子的评论
    public List<PostComment>? PostComments { get; set; }
}