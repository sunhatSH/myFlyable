using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Flyable.Repositories.DataAccess.DataBaseAccess.Access;

public class PostAccess : IPostAccess
{
    public PostAccess(FlyableUserContext flyableUserContext)
    {
        Context = flyableUserContext;
    }

    private FlyableUserContext Context { get; }

    public async Task<int> AddPost(Post post)
    {
        Console.WriteLine(JsonConvert.SerializeObject(post));
        await Context.Posts!.AddAsync(post);
        return 0;
    }

    public async Task<int> DeletePost(int postId)
    {
        return Context.Posts?
            .Remove(await Context.Posts.FindAsync(postId) ?? throw new Exception("Post Not Found"))
            .Collections.Count() ?? 0;
    }

    /// <summary>
    ///     更新帖子
    /// </summary>
    /// <param name="post"></param>
    /// <returns>返回更改的数据数量，小于等于0失败</returns>
    public async Task<int> UpdatePost(Post post)
    {
        return await Context.Posts!
            .ExecuteUpdateAsync(calls
                => calls.SetProperty(p => p.Content, post.Content)
                    .SetProperty(p => p.Title, post.Title)
                    .SetProperty(p => p.PostTags, post.PostTags)
                    .SetProperty(p => p.IsRecommend, post.IsRecommend)
                    .SetProperty(p => p.IsHot, post.IsHot)
                    .SetProperty(p => p.LastModifyTime, post.LastModifyTime)
                    .SetProperty(p => p.PublishTime, post.PublishTime)
                    .SetProperty(p => p.LikeCount, post.LikeCount)
                    .SetProperty(p => p.ReportCount, post.ReportCount));
    }

    public async Task<Post> FindPost(int postId)
    {
        return await Context.Posts?.FirstOrDefaultAsync(post => post.PostId == postId)! ??
               throw new Exception("Post Not Found");
    }

    public async Task<List<Post>?> FindPostsByUserId(int userId)
    {
        // return await (from post in Context.Posts where post.UserId == userId select post).ToListAsync();
        return await Context.Posts?.Where(post => post.UserId == userId).ToListAsync()! ??
               throw new Exception("Post Not Found");
    }

    public async Task<List<Post>> FindPostsByTag(PostTag tag)
    {
        return await Context.Posts?.Where(post => post.PostTags.Contains(tag)).ToListAsync()! ??
               throw new Exception("Post Not Found");
    }

    public async Task<List<Post>> FindPostsByAuthor(string authorName)
    {
        return await Context.Posts?.Where(post => post.Username == authorName).ToListAsync()! ??
               throw new Exception("Post Not Found");
    }

    public async Task<List<Post>> FindCollections(int userId)
    {
        return (await Context.Users
                   .Include(user => user.CollectionPosts)
                   .FirstOrDefaultAsync(user => user.UserId == userId))?.CollectionPosts
               ?? new List<Post>();
    }

    /// <summary>
    ///     暂时不解决性能问题，以后有需要再解决
    ///     解决方案： 将查询的条件改为上一个查询结果集最后的帖子ID，而不是页数，使用where子句
    /// </summary>
    /// <param name="pageNum"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<List<Post>?> FindAllPagedPosts(int pageNum, int pageSize)
    {
        return await Context.Posts?
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync()!;
    }

    public async Task<List<Post>?> FindPostsByKeys(params string[] keys)
    {
        var result = new List<Post>();
        foreach (var key in keys)
            result.AddRange(
                await Context.Posts?
                    .Where(post => EF.Functions.Like(post.Title!, $"%{key}%")
                                   || EF.Functions.Like(post.Content!, $"%{key}%")
                                   || EF.Functions.Like(post.Username!, $"%{key}%")
                                   || EF.Functions.Like(post.PostTags.ToString()!, $"%{key}%")
                    )
                    .ToListAsync()!);
        return result;
    }
}