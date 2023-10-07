using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Repositories.Entities;
using Flyable.Services.IServices;

namespace Flyable.Services.Services;

public class PostService : IPostService
{
    public PostService(IPostAccess postAccess)
    {
        PostAccess = postAccess;
    }

    public IPostAccess PostAccess { get; set; }

    public async Task<int> PublishPost(Post post)
    {
        await PostAccess.AddPost(post);
        return 0;
    }

    public async Task<int> DeletePost(int postId)
    {
        return 0;
    }

    public async Task<int> UpdatePost(Post post)
    {
        return 0;
    }

    public async Task<Post> FindPost(int postId)
    {
        return null;
    }

    public async Task<List<Post>> FindPostsByUserId(int userId)
    {
        return null;
    }

    public async Task<List<Post>> FindPostsByTag(string tag)
    {
        return null;
    }

    public async Task<List<Post>> FindPostsByAuthor(string authorName)
    {
        return null;
    }

    public async Task<List<Post>> FindCollections(int userId)
    {
        return null;
    }

    public async Task<int> LikePost(int postId)
    {
        return 0;
    }

    public async Task<int> UnLikePost(int postId)
    {
        return 0;
    }

    public async Task<int> CollectPost(int postId)
    {
        return 0;
    }

    public async Task<int> UnCollectPost(int postId)
    {
        return 0;
    }

    public async Task<int> RewardPost(int postId, int rewardNum, int rewardType)
    {
        return 0;
    }

    public async Task<int> AddTag(int postId, string tagName)
    {
        return 0;
    }

    public async Task<int> DeleteTag(int postId, string tagName)
    {
        return 0;
    }

    public async Task<List<Post>> FindAllPosts()
    {
        return null;
    }

    public async Task<List<Post>> FindPostsByKeys(params string[] keys)
    {
        return null;
    }

    public async Task<int> RecommendMyPost(string postName, string reason)
    {
        return 0;
    }
}