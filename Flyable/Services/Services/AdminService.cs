using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Repositories.Entities;
using Flyable.Services.IServices;
using Flyable.Services.ViewModels;

namespace Flyable.Services.Services;

public class AdminService : IAdminService
{
    public AdminService(IAdminAccess adminAccess)
    {
        AdminAccess = adminAccess;
    }

    private IAdminAccess AdminAccess { get; }


    public async Task<int> RecommendAdmin(int userId, string reason)
    {
        return 0;
    }

    public async Task<int> Resign(int userId, string reason)
    {
        return 0;
    }

    public async Task<int> PublishNotification(string content, int targetId)
    {
        return 0;
    }

    public async Task<int> WithdrawNotification(int notificationId)
    {
        return 0;
    }

    public async Task<int> SendMessage(string content, int targetId)
    {
        return 0;
    }

    public async Task<int> WithdrawMessage(int messageId)
    {
        return 0;
    }

    public async Task<int> BanUser(int userId, string reason)
    {
        return 0;
    }

    public async Task<int> UnbanUser(int userId)
    {
        return 0;
    }

    public async Task<int> MuteUser(int userId, string reason)
    {
        return 0;
    }

    public async Task<int> UnmuteUser(int userId)
    {
        return 0;
    }

    public async Task<int> BanNovel(int novelId, string reason)
    {
        return 0;
    }

    public async Task<int> UnbanNovel(int novelId)
    {
        return 0;
    }

    public async Task<int> BanPost(int postId, string reason)
    {
        return 0;
    }

    public async Task<int> UnbanPost(int postId)
    {
        return 0;
    }

    public async Task<int> BanComment(int commentId, string reason)
    {
        return 0;
    }

    public async Task<int> UnbanComment(int commentId)
    {
        return 0;
    }

    public async Task<int> BanChapter(int chapterId, string reason)
    {
        return 0;
    }

    public async Task<int> UnbanChapter(int chapterId)
    {
        return 0;
    }

    public async Task<int> UpdateUser(User user, string reason)
    {
        return 0;
    }

    public async Task<int> PublishUserTag(UserTagModelView tag)
    {
        return await AdminAccess.PublishUserTag(tag);
    }
}