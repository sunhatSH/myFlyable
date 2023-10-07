using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Repositories.Entities;
using Flyable.Services.ViewModels;

namespace Flyable.Repositories.DataAccess.DataBaseAccess.Access;

public class AdminAccess : IAdminAccess
{
    public AdminAccess(FlyableUserContext context)
    {
        Context = context;
    }

    private FlyableUserContext Context { get; }

    public async Task<int> PublishUserTag(UserTagModelView tag)
    {
        await using (Context)
        {
            Context.UserTags.Add(new UserTag
            {
                TagName = tag.TagName,
                TagDescription = tag.TagDescription,
                CreatedTime = DateTime.Now,
                TagIcon = tag.TagIcon,
                TagRemain = tag.TagRemain,
                LastPublishTime = DateTime.Now
            });
            return await Context.SaveChangesAsync();
        }
    }
}