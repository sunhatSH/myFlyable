using System.ComponentModel.DataAnnotations;
using Flyable.Dtos; // 确保可以引用 CommonDtos.cs

namespace Flyable.Dtos;

/// <summary>
/// 创建小说DTO
/// </summary>
public class CreateNovelDto
{
    [Required(ErrorMessage = "小说标题不能为空")]
    [StringLength(100, ErrorMessage = "标题长度不能超过100个字符")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "小说描述不能为空")]
    [StringLength(1000, ErrorMessage = "描述长度不能超过1000个字符")]
    public string Description { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "封面图片URL长度不能超过500个字符")]
    public string? CoverImage { get; set; }

    [Required(ErrorMessage = "小说分类不能为空")]
    [StringLength(50, ErrorMessage = "分类长度不能超过50个字符")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "作者ID不能为空")]
    public int AuthorId { get; set; }
}

/// <summary>
/// 更新小说DTO
/// </summary>
public class UpdateNovelDto
{
    [StringLength(100, ErrorMessage = "标题长度不能超过100个字符")]
    public string? Title { get; set; }

    [StringLength(1000, ErrorMessage = "描述长度不能超过1000个字符")]
    public string? Description { get; set; }

    [StringLength(500, ErrorMessage = "封面图片URL长度不能超过500个字符")]
    public string? CoverImage { get; set; }

    [StringLength(50, ErrorMessage = "分类长度不能超过50个字符")]
    public string? Category { get; set; }

    /// <summary>
    /// 小说状态：0正常 1软删除 2申请软删除 3被编辑锁定 4被作者隐藏
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// 是否完结
    /// </summary>
    public bool? IsFinished { get; set; }

    public string? Copywriting { get; set; }
    public string? SellingPoint { get; set; }
    public string? Recommendation { get; set; }
}

/// <summary>
/// 创建章节DTO
/// </summary>
public class CreateChapterDto
{
    [Required(ErrorMessage = "章节标题不能为空")]
    [StringLength(100, ErrorMessage = "标题长度不能超过100个字符")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "章节内容不能为空")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "章节序号不能为空")]
    [Range(1, int.MaxValue, ErrorMessage = "章节序号必须大于0")]
    public int ChapterNumber { get; set; }
}

/// <summary>
/// 小说评分DTO
/// </summary>
public class RateNovelDto
{
    [Required(ErrorMessage = "评分不能为空")]
    [Range(1, 5, ErrorMessage = "评分必须在1-5之间")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "用户ID不能为空")]
    public int UserId { get; set; }
}

public class NovelCollectionDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? AuthorName { get; set; }
    public string? Cover { get; set; }
    public string? ShortDescription { get; set; }
    public double Score { get; set; }
    public int CollectNums { get; set; }
    public bool IsFinished { get; set; }
    public DateTime LastAlterTime { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int RatingCount { get; set; }
}

/// <summary>
/// 小说查询DTO
/// </summary>
public class NovelQueryDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Keyword { get; set; }
    public string? Category { get; set; }
    public bool? IsFinished { get; set; }
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; } = true;
}

/// <summary>
/// 小说列表项DTO
/// </summary>
public class NovelListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string? Cover { get; set; }
    public string? ShortDescription { get; set; }
    public double Score { get; set; }
    public int CollectNums { get; set; }
    public bool IsFinished { get; set; }
    public DateTime LastAlterTime { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int RatingCount { get; set; }
    public double Hot { get; set; }
}

public class CopywritingDto
{
    public string? Copywriting { get; set; }
    public string? SellingPoint { get; set; }
    public string? Recommendation { get; set; }
}