using Flyable.Repositories.Entities;
using Flyable.Services.ViewModels;

namespace Flyable.Services.IServices;

public interface IAdminService
{
    // 推荐新管理
    public Task<int> RecommendAdmin(int userId, string reason);

    // 辞职
    public Task<int> Resign(int userId, string reason);

    // 发布通知
    public Task<int> PublishNotification(string content, int targetId);

    // 撤回通知
    public Task<int> WithdrawNotification(int notificationId);

    // 发送消息，群聊、私聊
    public Task<int> SendMessage(string content, int targetId);

    // 撤回消息
    public Task<int> WithdrawMessage(int messageId);

    // 封禁用户
    public Task<int> BanUser(int userId, string reason);

    // 解封用户
    public Task<int> UnbanUser(int userId);

    // 禁言用户
    public Task<int> MuteUser(int userId, string reason);

    // 解除禁言
    public Task<int> UnmuteUser(int userId);

    // 封禁小说
    public Task<int> BanNovel(int novelId, string reason);

    // 解封小说
    public Task<int> UnbanNovel(int novelId);

    // 封禁帖子
    public Task<int> BanPost(int postId, string reason);

    // 解封帖子
    public Task<int> UnbanPost(int postId);

    // 封禁评论
    public Task<int> BanComment(int commentId, string reason);

    // 解封评论
    public Task<int> UnbanComment(int commentId);

    // 封禁章节
    public Task<int> BanChapter(int chapterId, string reason);

    // 解封章节
    public Task<int> UnbanChapter(int chapterId);

    // 修改用户信息
    public Task<int> UpdateUser(User user, string reason);


    public Task<int> PublishUserTag(UserTagModelView tag);
}