using Flyable.Repositories.Entities;

namespace Flyable.Services.IServices;

public interface ICommentService
{
    // 发布评论
    public Task<int> PublishComment(string content, Type targetType, int targetId);

    // 删除评论
    public Task<int> DeleteComment(int commentId, Type targetType);

    // 修改评论
    // commentId: 评论id， targetType: 评论对象类型， content: 修改后的评论内容
    // 评论ID和评论对象类型确定了唯一的评论
    public Task<int> UpdateComment(int commentId, Type targetType, string content);

    // 点赞评论
    public Task<int> LikeComment(int commentId, Type targetType);

    // 取消点赞评论
    public Task<int> DislikeComment(int commentId, Type targetType);

    // 举报评论
    public Task<int> ReportComment(int commentId, Type targetType, string reason);

    // 打赏评论
    public Task<int> RewardComment(int commentId, Type targetType, int rewardNum, int rewardType);

    // 显示文章所有评论
    public Task<List<ChapterComment>> FindCommentsByChapterId(int chapterId);

    // 显示帖子所有评论
    public Task<List<PostComment>> FindCommentsByPostId(int postId);

    // 显示小说所有评论
    public Task<List<NovelComment>> FindCommentsByNovelId(int novelId);
}