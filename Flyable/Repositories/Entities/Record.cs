using System.ComponentModel.DataAnnotations;

namespace Flyable.Repositories.Entities;

public class AdminRecord
{
    [Key] public int RecordId { get; set; }

    public int OperatorTypeCode { get; set; }

    // /// <summary>//在控制层做这个
    // /// 操作描述
    // /// </summary>
    // public string? Detail => Enum.GetName(OperateType);

    // 该操作的时间
    public DateTime OperateTime { get; set; }

    // 该操作的管理员ID
    public int AdminId { get; set; }

    // 该操作的管理员名
    public string AdminName { get; set; } = null!;

    // 该操作的管理员是否已辞职
    public bool IsResigned { get; set; }

    // 该操作的详情
    public string? Detail { get; set; }

    // 该操作的理由
    public string? Reason { get; set; }
}

public class EditorRecord
{
    [Key] public int RecordId { get; set; }

    public int OperateTypeCode { get; set; }

    // /// <summary>//在控制层做这个
    // /// 操作描述
    // /// </summary>
    // public string? Detail => Enum.GetName(OperateType);

    // 该操作的时间
    public DateTime OperateTime { get; set; }

    // 该操作的编辑ID
    public int EditorId { get; set; }

    // 该操作的编辑名
    public string EditorName { get; set; } = null!;

    // 该操作的编辑是否已辞职
    public bool IsResigned { get; set; }

    // 该操作的详情
    public string? Detail { get; set; }

    // 该操作的理由
    public string? Reason { get; set; }
}