using Flyable.Services.ViewModels;

namespace Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;

public interface IAdminAccess
{
    public Task<int> PublishUserTag(UserTagModelView tag);
}