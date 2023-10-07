using Microsoft.EntityFrameworkCore;

namespace Flyable.Repositories.Entities;

[Index(nameof(ApplyId))]
[Index(nameof(UserId))]
public class Apply
{
    // 申请id,用来维护索引,加快索引
    public int ApplyId { get; set; }

    // 申请内容
    public string ApplyContent { get; set; } = null!;

    // 申请人id
    public int UserId { get; set; }

    // 申请时间
    public DateTime ApplyTime { get; set; }

    // 申请状态 0 未处理 1 已同意 2 已拒绝
    public int ApplyStatus { get; set; }

    // 申请类型 0 申请成为编辑 1 申请成为管理员
    public int ApplyType { get; set; }

    // 申请被处理的时间, 若未处理则为null
    public DateTime? ProcessTime { get; set; }
}