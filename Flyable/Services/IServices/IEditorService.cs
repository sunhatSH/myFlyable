namespace Flyable.Services.IServices;

public interface IEditorService
{
    // 推荐小说
    public Task<int> RecommendNovel(string novelId);

    // 推荐帖子
    public Task<int> RecommendPost(string postId);

    // 锁定小说
    public Task<int> LockNovel(string novelId);

    // 锁定帖子
    public Task<int> LockPost(string postId);

    // 锁定评论
    public Task<int> LockComment(string commentId);

    // 锁定章节
    public Task<int> LockChapter(string chapterId);

    // 推荐新编辑
    public Task<int> RecommendEditor(string userId);

    // 辞职
    public Task<int> Resign(string userId);

    // 发布通知
    public Task<int> PublishNotification(string content, int targetId);

    // 撤回通知
    public Task<int> WithdrawNotification(int notificationId);

    // 发送消息
    public Task<int> SendMessage(string content, int targetId);

    // 撤回消息
    public Task<int> WithdrawMessage(int messageId);
}