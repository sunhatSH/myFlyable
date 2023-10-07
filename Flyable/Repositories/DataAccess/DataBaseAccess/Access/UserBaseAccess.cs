using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Repositories.Entities;
using Flyable.Services.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Flyable.Repositories.DataAccess.DataBaseAccess.Access;

public class UserBaseAccess : IUserBaseAccess
{
    public UserBaseAccess(FlyableUserContext flyableUserContext)
    {
        Context = flyableUserContext;
    }

    private FlyableUserContext Context { get; }

    public async Task<List<User>> FindAllUsers()
    {
        return await Context.Users.ToListAsync();
    }

    public async Task<int> FindUserByUsername(string username, string pwdSecret)
    {
        return (await Context.Users.FirstOrDefaultAsync(user =>
            user.Username == username && user.PwdToken == pwdSecret))?.Status ?? -1;
    }

    public async Task<UserModelView?> AddUser(UserModelView userModelView, string pwdSecret)
    {
        var emails = await Context.Users.Select(user => user.Email).ToListAsync();
        if (emails.Contains(userModelView.Email)) return null;
        var user = new User
        {
            Username = userModelView.Username,
            Email = userModelView.Email,
            PwdToken = pwdSecret,
            Status = userModelView.Status,
            HeadLink = userModelView.HeadLink,
            SelfIntroduction = userModelView.SelfIntroduction,
            TagList = new List<UserTag>
            {
                new()
                {
                    TagName = userModelView.UserTag.TagName,
                    TagDescription = userModelView.UserTag.TagDescription,
                    TagIcon = userModelView.UserTag.TagIcon,
                    TagRemain = userModelView.UserTag.TagRemain,
                    CreatedTime = DateTime.Now,
                    LastPublishTime = DateTime.Now
                }
            }
        };
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        return userModelView;
    }

    public async Task<UserModelView> GetUserModelViewByUsername(string username)
    {
        return (await Context.Users
            .Select(user
                => new UserModelView(user.Username!, user.Email ?? "", user.Status, user.HeadLink ?? "",
                    user.SelfIntroduction ?? "",
                    new UserTagModelView(user.WearTag.TagName ?? string.Empty,
                        user.WearTag.TagDescription ?? string.Empty, user.WearTag.TagIcon ?? string.Empty,
                        user.WearTag.TagRemain))).FirstOrDefaultAsync())!;
    }

    public async Task<bool> IsAuthorOrAdmin(int userId, Type targetType, int commentId)
    {
        var auth = await Context.Users
            .Select(user => new { user.Role, user.Status, user.UserId })
            .Where(user => user.UserId == userId)
            .SingleAsync();
        return auth is { Status: 0 or 2 or 4 or 6 } or { Role: <= 6 }; // 作者无法删除其他人评论 （7： 作者）
    }
}