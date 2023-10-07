using System.ComponentModel.DataAnnotations.Schema;

namespace Flyable.Repositories.Entities;

/// <summary>
///     举报小说、章节、评论、用户
/// </summary>
public class Report
{
    public int ReportId { get; set; }

    // 举报对象的类型 0:小说 1:章节 2:帖子 3:小说评论 4:章节评论 5:帖子评论 6:用户 7:管理员 8:编辑
    public int ReportType { private get; set; }

    [NotMapped]
    public string ReportDict => ReportType switch
    {
        0 => "小说",
        1 => "章节",
        2 => "帖子",
        3 => "小说评论",
        4 => "章节评论",
        5 => "帖子评论",
        6 => "用户",
        7 => "管理员",
        8 => "编辑",
        var _ => "未知"
    };

    // 举报者的id
    public int ReporterId { get; set; }

    // 被举报者的ID，如果是小说等，就是该小说的ID，如果是用户、管理，就是用户、管理的ID
    public int ReportedId { get; set; }

    // 举报理由
    public string? Reason { get; set; }

    // 举报时间
    public DateTime ReportTime { get; set; }

    // 举报状态 0:未处理 1: 已处理 2:驳回
    public int Status { get; set; }

    // 处理时间
    public DateTime? ProcessTime { get; set; }

    // 处理者的ID
    public int? ProcessorId { get; set; }
}