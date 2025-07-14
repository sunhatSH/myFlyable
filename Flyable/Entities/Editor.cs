namespace Flyable.Entities;

public class Editor
{
    // 编辑组成员id,用来维护索引,加快索引
    public int EditorId { get; set; }

    // 编辑组成员的用户id,与用户表关联
    public int UserId { get; set; }

    // 编辑名称
    public string? EditorName { get; set; }


    // 编辑组成员的状态 0 正常 1 暂时禁止操作 2 已辞职
    public int Status { get; set; }

    // 编辑组成员的加入时间
    public DateTime JoinTime { get; set; }

    // 上一次修改状态的时间
    public DateTime LastModifyTime { get; set; }
}