using Flyable.Repositories.Entities;
using Flyable.Services.ViewModels;

namespace Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;

public interface IUserBaseAccess
{
    /// <summary>
    ///     查找所有用户
    /// </summary>
    /// <returns></returns>
    public Task<List<User>> FindAllUsers();

    public Task<int> FindUserByUsername(string username, string pwdSecret);
    public Task<UserModelView?> AddUser(UserModelView userModelView, string pwdSecret);
    public Task<UserModelView> GetUserModelViewByUsername(string username);
    Task<bool> IsAuthorOrAdmin(int userId, Type targetType, int commentId);
}