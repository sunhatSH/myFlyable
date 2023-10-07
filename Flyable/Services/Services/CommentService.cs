using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Repositories.Entities;
using Flyable.Services.IServices;
using NUnit.Framework;

namespace Flyable.Services.Services;

public class CommentService : ICommentService
{
    private readonly ICommentAccess _commentAccess;
    private readonly IUserBaseAccess _userBaseAccess;
    private readonly IPostAccess _postAccess;
    public CommentService(ICommentAccess commentAccess, IUserBaseAccess userBaseAccess)
    {
        _commentAccess = commentAccess;
        _userBaseAccess = userBaseAccess;
    }


    public async Task<int> PublishComment(string content, Type targetType, int targetId)
    {
        if (targetType == typeof(Post))
            return await _commentAccess.AddPostCommentAsync(
                new PostComment
                {
                    Content = content,
                    UserId = 1, //TODO: 从token中获取
                    PublishTime = DateTime.Now
                });
        if (targetType == typeof(Chapter))
            return await _commentAccess.AddChapterCommentAsync(
                new ChapterComment
                {
                    Content = content,
                    UserId = 1, //TODO: 从token中获取
                    PublishTime = DateTime.Now
                });
        if (targetType == typeof(Novel))
            return await _commentAccess.AddNovelCommentAsync(
                new NovelComment
                {
                    Content = content,
                    UserId = 1, //TODO: 从token中获取
                    PublishTime = DateTime.Now
                });
        throw new Exception("未知目标类型，无法评论");
    }

    public async Task<int> DeleteComment(int commentId, Type targetType)
    {
        const int userId = 1; //TODO: 从token中获取
        var isAllowed = await _userBaseAccess.IsAuthorOrAdmin(userId, targetType, commentId);
        if (!isAllowed) return -1;
        if (targetType == typeof(Post)) return await _commentAccess.DeletePostCommentAsync(commentId);

        if (targetType == typeof(Chapter)) return await _commentAccess.DeleteChapterCommentAsync(commentId);

        if (targetType == typeof(Novel)) return await _commentAccess.DeleteNovelCommentAsync(commentId);

        if (targetType == typeof(ChapterComment))
            return await _commentAccess.DeleteChapterCommentReplyAsync(commentId);

        if (targetType == typeof(PostComment)) return await _commentAccess.DeletePostCommentReplyAsync(commentId);

        if (targetType == typeof(NovelComment)) return await _commentAccess.DeleteNovelCommentReplyAsync(commentId);
        throw new Exception("未知目标类型，无法删除");
    }

    public async Task<int> UpdateComment(int commentId, Type targetType, string content)
    {
        const int userId = 1; //TODO: 从token中获取
        var isAllowed = await _userBaseAccess.IsAuthorOrAdmin(userId, targetType, commentId);
        if (!isAllowed) return -1;
        if (targetType == typeof(Post)) return await _commentAccess.UpdatePostCommentAsync(commentId, content);

        if (targetType == typeof(Chapter)) return await _commentAccess.UpdateChapterCommentAsync(commentId, content);

        if (targetType == typeof(Novel)) return await _commentAccess.UpdateNovelCommentAsync(commentId, content);

        if (targetType == typeof(ChapterComment))
            return await _commentAccess.UpdateChapterCommentReplyAsync(commentId, content);

        if (targetType == typeof(PostComment))
            return await _commentAccess.UpdatePostCommentReplyAsync(commentId, content);

        if (targetType == typeof(NovelComment))
            return await _commentAccess.UpdateNovelCommentReplyAsync(commentId, content);
        throw new Exception("未知目标类型，无法更新");
    }

    public async Task<int> LikeComment(int commentId, Type targetType)
    {
        return await _commentAccess.LikeCommentAsync(commentId, targetType);
    }

    public async Task<int> DislikeComment(int commentId, Type targetType)
    {
        return await _commentAccess.DislikeCommentAsync(commentId, targetType);
    }

    public async Task<int> ReportComment(int commentId, Type targetType, string reason)
    {
        return 0;
    }

    public async Task<int> RewardComment(int commentId, Type targetType, int rewardNum, int rewardType)
    {
        return 0;
    }

    public async Task<List<ChapterComment>> FindCommentsByChapterId(int chapterId)
    {
        return null;
    }

    public async Task<List<PostComment>> FindCommentsByPostId(int postId)
    {
        return null;
    }

    public async Task<List<NovelComment>> FindCommentsByNovelId(int novelId)
    {
        return null;
    }
}