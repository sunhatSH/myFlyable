namespace Flyable.StatusCode;

public struct ContentStatusCode
{
    //创建小说/章节/评论/帖子
    public const int Created = 2001;
    public const int CreateFailed = 2002;

    public const int Altered = 2003;

    public const int AlterFailed = 2004;
    //访问小说/章节
    //用户等级不足

    public const int LevelNotEnough = 2005;

    //用户身份错误
    public const int IdentityError = 2006;

    // 其他原因无法访问, 如果使用这个状态码, 请在返回的json中加入一个reason字段, 用于说明原因
    public const int OtherReason = 2007;
}

public struct ContentType
{
    public const int Post = 0;
    public const int Chapter = 1;
    public const int Novel = 2;
    public const int ChapterComment = 3;
    public const int PostComment = 4;
    public const int NovelComment = 5;
}