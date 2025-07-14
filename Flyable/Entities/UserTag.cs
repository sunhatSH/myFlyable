using System.ComponentModel.DataAnnotations;

namespace Flyable.Entities;

public class UserTag
{
    // TagId
    [Key] public int TagId { get; set; }

    // Tag名称
    public string? TagName { get; set; }

    // Tag描述
    public string? TagDescription { get; set; }

    // Tag 创造时间
    public DateTime CreatedTime { get; set; }

    // Tag Tag的Icon
    public string? TagIcon { get; set; }

    // Tag的余量
    public int TagRemain { get; set; }

    // Tag上一次发放时间
    public DateTime LastPublishTime { get; set; }
}