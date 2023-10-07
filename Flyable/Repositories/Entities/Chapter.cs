namespace Flyable.Repositories.Entities;

public class Chapter
{
    //章节ID是所有小说总章节的唯一标识,章节顺序由另一个字段 ChapterOrder 记录
    public int ChapterId { get; set; }

    public string? ChapterName { get; set; }

    // 章节速记
    public string? Shorthand { get; set; }

    // 章节内容
    public string? Content { get; set; }

    // 所属小说的ID
    public int NovelId { get; set; }

    //章节字数
    public int WordCount { get; set; }

    //章节浏览量
    public int ViewCount { get; set; }

    //章节点赞数
    public int LikeCount { get; set; }

    //章节评论数
    public int CommentCount => ChapterComments?.Count ?? 0;

    //章节创建时间
    public DateTime CreateTime { get; set; }

    //章节最后修改时间
    public DateTime LastAlterTime { get; set; }

    /// <summary>
    ///     章节状态
    ///     定时发布功能由浏览器端存储
    ///     0:正常 1:软删除 2:被管理员或者编辑锁定 3 被作者隐藏
    /// </summary>
    public int ChapterStatus { get; set; }

    // 章节评论
    public List<ChapterComment>? ChapterComments { get; set; }
    //章节顺序
    public int ChapterOrder { get; set; }

    // 章节末尾的作者的话
    public string? AuthorWords { get; set; }


}