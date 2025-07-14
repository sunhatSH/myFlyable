using System.ComponentModel.DataAnnotations.Schema;
using Serilog;

namespace Flyable.Entities;

public class Novel
{
    // 小说ID
    public int NovelId { get; set; }

    // 小说名 拼音检索功能不提供
    public string? NovelName { get; set; }

    /// <summary>
    ///     修改之前的小说名,可以给刚刚改名的小说提供检索帮助,避免刚改名的小说被埋没
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    ///     简短介绍
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    ///     详细介绍
    /// </summary>
    public string? DetailedDescription { get; set; }


    /// <summary>
    ///     作者ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    ///     作者名,将需要频繁访问的属性直接放进小说表里,避免频繁联表查询  //使用视图解决
    /// </summary>
    public string AuthorName { get; set; } = null!;

    /// <summary>
    ///     收藏数量
    /// </summary>
    public int FavoredNums { get; set; }

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    ///     最近修改书籍信息时间
    /// </summary>
    public DateTime LastAlterTime { get; set; }

    /// <summary>
    ///     读者评分
    /// </summary>
    public double Score { get; set; }

    /// <summary>
    ///     评分人数
    /// </summary>
    public int ScorePeopleNum { get; set; }

    /// <summary>
    ///     小说状态 0 正常 1 被管理员软删除 2申请软删除 3 被编辑锁定 4 被作者隐藏
    /// </summary>
    public int NovelStatus { get; set; }

    //喜欢数量 等于所有章节以及小说本身喜欢数之和
    public int LikeNums { get; set; }

    //收藏数量
    public int CollectNums { get; set; }

    // 评论
    public List<NovelComment>? NovelComments { get; set; }
    //评论数量 等于所有章节以及小说本身评论数之和
    public int CommentNums => NovelComments?.Count ?? 0;

    //字数 等于所有章节字数之和
    public int WordCount { get; set; }

    //点击量
    public int TotalClicks { get; set; }

    //总获得的打赏羽毛数量
    public int TotalFeather { get; set; }

    //总获得的打赏彩色石数量
    public int TotalColorStone { get; set; }

    /// <summary>
    ///     是否开启聊天室 true:开启 false:关闭
    ///     当开启聊天室时,小说聊天室将等同于小说的评论区
    ///     聊天室的消息将会被当作评论,小说的评论将会被当作聊天室消息
    /// </summary>
    public bool IsOpenChatRoom { get; set; }

    /// <summary>
    ///     是否允许评论 true:允许 false:不允许
    /// </summary>
    public bool IsAllowComment { get; set; }

    /// <summary>
    ///     是否被编辑推荐 true:被推荐 false:未被推荐
    /// </summary>
    public bool IsEditorRecommend { get; set; }

    // 编辑推荐语
    public string? EditorRecommendReason { get; set; }

    /// <summary>
    ///     是否完结 true:完结 false:连载中
    /// </summary>
    public bool IsFinished { get; set; }

    /// <summary>
    ///     小说封面
    /// </summary>
    public string? Cover { get; set; }

    /// <summary>
    ///     小说类型 0 网游 1 玄幻 2 都市 3 科幻 4 灵异 5 历史 6 穿越 7 重生 8 竞技 9 其他
    /// </summary>
    public int NovelType { get; set; }

    /// <summary>
    ///     小说标签,支持自定义和使用预定标签(刚上线只有自定义,后续会有专用标签)
    /// </summary>
    public List<NovelTag>? NovelTags { get; set; }

    //小说时代背景 0 古代 1 现代 2 未来 3 其他
    public int NovelBackground { get; set; }

    //小说性取向 0 言情 1 耽美 2 百合 3 无CP
    public int NovelOrientation { get; set; }

    //是否原创 true:原创 false:非原创
    public bool IsOriginal { get; set; }

    // 小说是否在首页显示
    public bool IsShowOnHomePage { get; set; }

    // 上次管理时间
    public DateTime LastModifyTime { get; set; }

    // 上一个提交修改小说状态的管理员ID
    public int LastModifyAdminId { get; set; }

    // 小说被举报次数
    public int ReportNums { get; set; }

    // 小说一句话文案
    public string? Copywriting { get; set; }
    // 小说卖点
    public string? SellingPoint { get; set; }
    // 推荐语
    public string? Recommendation { get; set; }

    public List<User> LikeUsers { get; set; } = new();

    public List<User> CollectionUsers { get; set; } = new();

    public List<NovelRating>? NovelRatings { get; set; }

    public double Hot { get; set; }

    // 静态热度计算方法
    public static double CalculateHot(Novel n)
    {
        if (n == null) return 0;
        var hot = n.TotalClicks * 0.1f +
                  n.LikeNums * 0.2f +
                  n.CommentNums * 0.8f +
                  n.CollectNums * 0.3f +
                  n.TotalColorStone * 0.1f +
                  n.TotalFeather * 0.1f  +
                  (n.IsFinished ? Math.Log(Math.Max(n.WordCount, 1)) : Math.Log(Math.Max(n.WordCount / 2.0, 1))) +
                  (n.IsEditorRecommend ? 1.0f : 0.0f)
                  + (n.IsOpenChatRoom ? 1.0f : 0.0f)
                  + (n.IsAllowComment ? 1.0f : 0.0f)
                  + (n.IsOriginal ? 1.0f : 0.0f) / Math.Max(n.WordCount, 1) -
                  n.ReportNums * 0.3f -
                  (DateTime.Now - n.LastModifyTime).TotalDays * 0.1f -
                  (DateTime.Now - n.CreatedTime).TotalDays * 0.1f;
        Console.WriteLine($"""
                           hot is {hot},
                           total feather is {n.TotalFeather},
                           total color stone is {n.TotalColorStone},
                           total clicks is {n.TotalClicks},
                           like nums is {n.LikeNums},
                           comment nums is {n.CommentNums},
                           collect nums is {n.CollectNums},
                           is finished is {n.IsFinished},
                           is editor recommend is {n.IsEditorRecommend},
                           is open chat room is {n.IsOpenChatRoom},
                           is allow comment is {n.IsAllowComment},
                           is original is {n.IsOriginal},
                           word count is {n.WordCount},
                           report nums is {n.ReportNums},
                           last modify time is {n.LastModifyTime},
                           created time is {n.CreatedTime},
                           last alter time is {n.LastAlterTime},
                           score is {n.Score},
                           score people num is {n.ScorePeopleNum}
                           """);
        return hot;
    }
}