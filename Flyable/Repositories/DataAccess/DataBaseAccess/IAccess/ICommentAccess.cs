#define sun
using Flyable.Repositories.Entities;

namespace Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;

public interface ICommentAccess
{
    public Task<int> LikeCommentAsync(int commentId, Type targetType);

    #region PostComment region
    public Task<PostComment?> GetPostCommentAsync(int commentId);
    public Task<List<PostComment>> GetPostCommentsAsync(int postId, int pageNum, int pageSize, int orderBy);
    public Task<int> DeletePostCommentAsync(int commentId);
    public Task<int> AddPostCommentAsync(PostComment comment);

    public Task<int> UpdatePostCommentAsync(int commentId, string content);

    #endregion

    #region NovelComment region

    public Task<NovelComment?> GetNovelCommentAsync(int commentId);
    public Task<List<NovelComment>> GetNovelCommentsAsync(int novelId, int pageNum, int pageSize);
    public Task<List<NovelComment>> GetNovelCommentsAsync(int novelId, int pageNum, int pageSize, int orderBy);
    public Task<int> DeleteNovelCommentAsync(int commentId);
    public Task<int> AddNovelCommentAsync(NovelComment comment);

    public Task<int> UpdateNovelCommentAsync(int commentId, string content);

    #endregion

    #region ChapterComment region

    public Task<ChapterComment?> GetChapterCommentAsync(int commentId);
    public Task<List<ChapterComment>> GetChapterCommentsAsync(int chapterId, int pageNum, int pageSize);
    public Task<List<ChapterComment>> GetChapterCommentsAsync(int chapterId, int pageNum, int pageSize, int orderBy);
    public Task<int> DeleteChapterCommentAsync(int commentId);
    public Task<int> AddChapterCommentAsync(ChapterComment comment);

    public Task<int> UpdateChapterCommentAsync(int commentId, string content);

    #endregion

    #region PostCommentReply region

    public Task<PostComment?> GetPostCommentReplyAsync(int commentId);
    public Task<List<PostComment>> GetPostCommentRepliesAsync(int commentId, int pageNum, int pageSize, int orderBy);
    public Task<int> DeletePostCommentReplyAsync(int commentId);
    public Task<int> AddPostCommentReplyAsync(PostComment comment);

    public Task<int> UpdatePostCommentReplyAsync(int commentId, string content);

    #endregion

    #region NovelCommentReply region

    public Task<NovelComment?> GetNovelCommentReplyAsync(int commentId);
    public Task<List<NovelComment>> GetNovelCommentRepliesAsync(int commentId, int pageNum, int pageSize);
    public Task<List<NovelComment>> GetNovelCommentRepliesAsync(int commentId, int pageNum, int pageSize, int orderBy);
    public Task<int> DeleteNovelCommentReplyAsync(int commentId);
    public Task<int> AddNovelCommentReplyAsync(NovelComment comment);

    public Task<int> UpdateNovelCommentReplyAsync(int commentId, string content);

    #endregion

    #region ChapterCommentReply region

    public Task<ChapterComment?> GetChapterCommentReplyAsync(int commentId);
    public Task<List<ChapterComment>> GetChapterCommentRepliesAsync(int commentId, int pageNum, int pageSize);

    public Task<List<ChapterComment>> GetChapterCommentRepliesAsync(int commentId, int pageNum, int pageSize,
        int orderBy);

    public Task<int> DeleteChapterCommentReplyAsync(int commentId);
    public Task<int> AddChapterCommentReplyAsync(ChapterComment comment);

    public Task<int> UpdateChapterCommentReplyAsync(int commentId, string content);

    #endregion

    public Task<int> DislikeCommentAsync(int commentId, Type targetType);
}