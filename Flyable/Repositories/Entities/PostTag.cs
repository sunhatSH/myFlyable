using System.ComponentModel.DataAnnotations;

namespace Flyable.Repositories.Entities;

public class PostTag
{
    // TagId
    [Key] public int TagId { get; set; }

    // Tag名称
    public string? TagName { get; set; }

    // Tag描述
    public string? TagDescription { get; set; }

    // Tag 创造时间
    public DateTime CreatedTime { get; set; }

    // 拥有该Tag的小说
    public List<Post>? Posts { get; set; }
}