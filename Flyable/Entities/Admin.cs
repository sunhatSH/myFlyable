using Microsoft.EntityFrameworkCore;

namespace Flyable.Entities;

[Index(nameof(AdminId))]
public class Admin
{
    // 管理组成员id,用来维护索引,加快索引
    public int AdminId { get; set; }

    // 管理组成员的用户id,与用户表关联
    public int UserId { get; set; }

    // 管理组成员的权限,权限0-10,权限自然分组
    public int Permission { get; set; }


    // 管理组成员的状态 0 正常 1 暂时禁止操作 2 已辞职
    public int Status { get; set; }

    // 管理组成员的加入时间
    public DateTime JoinTime { get; set; }
}