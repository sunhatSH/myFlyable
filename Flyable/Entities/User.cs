using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Flyable.StatusCode;
using Microsoft.EntityFrameworkCore;

// ReSharper disable CollectionNeverUpdated.Global

namespace Flyable.Entities;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(UserId), IsUnique = true)]
[Index(nameof(Ip))]
public class User
{
    public int UserId { get; set; }

    /// <summary>
    ///     登陆使用username, username唯一
    /// </summary>
    [Required]
    [MaxLength(20)]
    [MinLength(4)]
    public string? Username { get; set; }

    /// <summary>
    /// 第三方账号，如微信、QQ、支付宝等。格式为：第三方平台名_第三方平台用户名
    /// </summary>
    [NotMapped]
    public Dictionary<ThirdPartyType, string>? ThirdPartyAccount { get; set; }

    [StringLength(maximumLength: 10000)]
    public string? ThirdPartyAccountJson
    {
        get => ThirdPartyAccount == null ? null : JsonSerializer.Serialize(ThirdPartyAccount);
        set => ThirdPartyAccount = value == null ? null : JsonSerializer.Deserialize<Dictionary<ThirdPartyType, string>>(value);
    }
    [StringLength(maximumLength: 30, MinimumLength = 4, ErrorMessage = "Email length must be between 4 and 30 characters.")]
    public string? Email { get; set; }

    [StringLength(maximumLength: 1000)]
    public string? PwdToken { get; set; }

    /// <summary>
    ///     头像图片链接
    /// </summary>
    [StringLength(512, ErrorMessage = "HeadLink length must be between 0 and 512 characters.")]
    public string? HeadLink { get; set; }

    /// <summary>
    ///     注册时间
    /// </summary>
    public DateTime RegisterTime { get; set; }

    /// <summary>
    ///     登陆时间, 每次登陆计算用户注册时长, 决定授予的头衔和等级
    /// </summary>
    public DateTime LastLoginTime { get; set; }

    /// <summary>
    ///     存储用户上一次登陆的ip信息, 存储在数据库里使用int类型,可以减少空间和传输时间
    /// </summary>
    [StringLength(maximumLength: 60, MinimumLength = 60, ErrorMessage = "LastIp length must be 60 characters.")]
    public string Ip { get; set; } = null!;

    /// <summary>
    ///     用户身份
    ///     0, 站长, 拥有所有权限
    ///     1, 顶级管理员, 拥有管理权限,可以管理编辑与用户
    ///     2-3待定
    ///     4, 编辑, 拥有推荐等权限,可以管理作者
    ///     5-6 待定
    ///     7, 作者
    ///     8, 读者
    /// </summary>
    public ushort Role { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    ///     等级,   范围 Lv0 - Lv10
    /// </summary>
    public UserLevel Level { get; set; }

    /// <summary>
    ///     是否是会员,赞助得会员
    /// </summary>
    public bool IsVip { get; set; }

    /// <summary>
    ///     自我描述,可为空
    /// </summary>
    [StringLength(128)]
    public string? SelfIntroduction { get; set; }

    /// <summary>
    ///     收藏夹, 被收藏的小说ID列表
    /// </summary>
    public List<Novel> CollectionNovels { get; set; } = new();

    /// <summary>
    ///     收藏的帖子
    /// </summary>
    public List<Post> CollectionPosts { get; set; } = new();

    /// <summary>
    ///     用户关注的作者id,使用string,取出来之后再解析
    /// </summary>
    public List<User>? Follows { get; set; }

    /// <summary>
    ///     关注该用户的所有用户id,允许用户关注自己
    /// </summary>
    public List<User>? Followed { get; set; }


    /// <summary>
    ///     虚拟货币彩色石头,赞助获得
    /// </summary>
    public int ColorStone { get; set; }

    /// <summary>
    ///     羽毛, 活动获得
    /// </summary>
    public int Feather { get; set; }

    // 上次更改用户状态时间
    public DateTime LastModifyTime { get; set; }

    // 上次更改用户状态的管理员id
    public Admin? LastModifyAdmin { get; set; }

    // 被举报次数
    public int ReportCount { get; set; }

    public List<Novel> LikedNovels { get; set; } = new();
}