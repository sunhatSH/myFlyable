// ReSharper disable MemberCanBePrivate.Global
namespace Flyable.StatusCode;

/// <summary>
/// 8, 站长, 拥有所有权限
/// <br/>
/// 7, 顶级管理员, 拥有管理权限,可以管理编辑与用户
/// <br/>
/// 5-6待定
/// <br/>
/// 4, 编辑, 拥有推荐等权限,可以管理作者
/// <br/>
/// 2-3 待定
/// <br/>
/// 1, 作者, 拥有发布文章和帖子的权限
/// <br/>
/// 0, 读者, 拥有浏览文章和帖子,发表评论的权限
/// <br/>
/// level k 的用户同时也是 level k-1 的用户
/// </summary>
public struct UserRole
{
    public const ushort Master   = 8;
    public const ushort TopAdmin = 7;
    public const ushort T1       = 6;
    public const ushort T2       = 5;
    public const ushort Editor   = 4;
    public const ushort T3       = 3;
    public const ushort T4       = 2;
    public const ushort Author   = 1;
    public const ushort Reader   = 0;
    public static readonly ushort[] All = [Master, TopAdmin, T1, T2, Editor, T3, T4, Author, Reader];
    public static readonly ushort[] GtEditor = [Master, TopAdmin, T1, T2, Editor];
    public static readonly ushort[] GtAuthor = [Master, TopAdmin, T1, T2, Editor, T3, T4, Author];
    public static readonly ushort[] OnlyMaster = [Master];
    public static readonly ushort[] GtAdmin = [Master, TopAdmin];
}