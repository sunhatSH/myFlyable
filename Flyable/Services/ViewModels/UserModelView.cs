using Flyable.Repositories.Entities;

namespace Flyable.Services.ViewModels;

public record UserModelView(string Username, string Email, int Status, string HeadLink, string SelfIntroduction,
    UserTagModelView UserTag);

public record CommentModelView(string Content, int TargetTypeId, int TargetId);

public record UserTagModelView(string TagName, string TagDescription, string TagIcon, int TagRemain);

public record PostModelView(string Title, string Content, string Username, int PostStatus, List<PostTag> PostTags,
    DateTime PublishTime, DateTime LastAlterTime, int LikeCount, int ReplyCount, bool IsRecommend,
    bool IsShowOnHomePage, bool IsHot, int TotalFeather, int TotalColorStone);

public record PostTagModelView(string TagName, string TagDescription);

public record PostCommentModelView(string Username, string Content, DateTime PublishTime, int LikeCount,
    int ReportCount, int CommentStatus, int ReplyCount, bool IsTop, bool IsRecommend, bool IsHot, bool IsReply,
    int ReplyToId);

public record NovelModelView(string Title, string Content, string Username, int NovelStatus, DateTime PublishTime,
    DateTime LastAlterTime, int LikeCount, int ReplyCount, bool IsRecommend, bool IsShowOnHomePage, bool IsHot,
    int TotalFeather, int TotalColorStone);