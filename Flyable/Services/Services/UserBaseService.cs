using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Repositories.DataAccess.MemoryAccess.Access;
using Flyable.Repositories.Entities;
using Flyable.Services.IServices;
using Flyable.Services.ViewModels;
using Flyable.StatusCode;

namespace Flyable.Services.Services;

public class UserBaseService : IUserBaseService
{
    public UserBaseService(IUserBaseAccess userBaseAccess)
    {
        UserBaseAccess = userBaseAccess;
    }

    private IUserBaseAccess UserBaseAccess { get; }
    private string? VerifyCode { get; set; }

    public async Task<UserModelView?> Register(UserModelView userModelView, string password, string verifyCode)
    {
        if (verifyCode != VerifyCode) return null;
        var user = await UserBaseAccess.AddUser(userModelView, password);
        return user;
    }


    public async Task<int> Login(string username, string pwdSecret, string verifyCode)
    {
        if (verifyCode != VerifyCode) return UserStatusCode.LoginFailed;
        var status = await UserBaseAccess.FindUserByUsername(username, pwdSecret);
        return status is >= 0 and <= 2
            ? UserStatusCode.Success
            : UserStatusCode.LoginFailed;
    }

    public async Task<int> DestroyAccount(string username, string pwdSecret, string verifyCode)
    {
        return 0;
    }

    public async Task<(int status, UserModelView newUser)> UpdateUser(params dynamic[] alteredItem)
    {
        return (0, null!);
    }

    public async Task<List<User>> FindAllUsers()
    {
        return await UserBaseAccess.FindAllUsers();
    }

    public async Task<int> FollowUser(string username)
    {
        return 0;
    }

    public async Task<int> UnFollowUser(int userId)
    {
        return 0;
    }

    public async Task<List<UserModelView>> FindUserByUsername(string username)
    {
        return null;
    }

    public async Task<UserModelView> FindUserByUserId(int userId)
    {
        return null;
    }

    public async Task<List<UserModelView>> FindUserByUserTag(UserTag userTag)
    {
        return null;
    }

    public async Task<int> ReportUser(string username, string reason)
    {
        return 0;
    }

    public string GetVerifyCode()
    {
        VerifyCode = VerifyCodeAccess.GetInstance().VerifyCode;

        return VerifyCode;
    }

    public async Task<UserModelView> GetUserModelView(string username)
    {
        return await UserBaseAccess.GetUserModelViewByUsername(username);
    }
}