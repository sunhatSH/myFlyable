using Flyable.Repositories.Entities;
using Flyable.Services.ViewModels;

namespace Flyable.Services.IServices;

/// <summary>
///     IUserService 负责用户的
///     注册、
///     登录、
///     注销、
///     修改个人信息、
///     查找用户等
///     用户信息通过Token传递
/// </summary>
public interface IUserBaseService
{
    /// <summary>
    ///     注册用户
    /// </summary>
    /// <param name="userModelView"></param>
    /// <param name="password"></param>
    /// <param name="verifyCode"></param>
    /// <returns></returns>
    public Task<UserModelView?> Register(UserModelView userModelView, string password, string verifyCode);

    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="username"></param>
    /// <param name="pwdSecret"></param>
    /// <param name="verifyCode"></param>
    /// <returns></returns>
    public Task<int> Login(string username, string pwdSecret, string verifyCode);

    /// <summary>
    ///     注销
    /// </summary>
    /// <param name="username"></param>
    /// <param name="pwdSecret"></param>
    /// <param name="verifyCode"></param>
    /// <returns></returns>
    public Task<int> DestroyAccount(string username, string pwdSecret, string verifyCode);

    /// <summary>
    ///     修改用户信息
    /// </summary>
    /// <param name="alteredItem"></param>
    /// <returns></returns>
    public Task<(int status, UserModelView newUser)> UpdateUser(params dynamic[] alteredItem);

    /// <summary>
    ///     查找所有用户
    /// </summary>
    /// <returns></returns>
    public Task<List<User>> FindAllUsers();

    /// <summary>
    ///     关注用户
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Task<int> FollowUser(string username);

    /// <summary>
    ///     取消关注用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<int> UnFollowUser(int userId);

    /// <summary>
    ///     通过username模糊查找用户
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Task<List<UserModelView>> FindUserByUsername(string username);

    /// <summary>
    ///     通过userId查找用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<UserModelView> FindUserByUserId(int userId);

    /// <summary>
    ///     通过userTag查找用户
    /// </summary>
    /// <param name="userTag"></param>
    /// <returns></returns>
    public Task<List<UserModelView>> FindUserByUserTag(UserTag userTag);

    /// <summary>
    ///     举报用户
    /// </summary>
    /// <param name="username"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public Task<int> ReportUser(string username, string reason);

    public string GetVerifyCode();

    public Task<UserModelView> GetUserModelView(string username);
}