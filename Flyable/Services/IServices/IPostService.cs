using Flyable.Repositories.Entities;

namespace Flyable.Services.IServices;

public interface IPostService
{
    // 发布帖子
    public Task<int> PublishPost(Post post);

    // 删除帖子
    public Task<int> DeletePost(int postId);

    // 修改帖子
    public Task<int> UpdatePost(Post post);

    // 查找帖子
    public Task<Post> FindPost(int postId);

    // 查找用户的所有帖子
    public Task<List<Post>> FindPostsByUserId(int userId);

    // 按帖子标签查找帖子
    public Task<List<Post>> FindPostsByTag(string tag);

    // 通过作者名查找帖子
    public Task<List<Post>> FindPostsByAuthor(string authorName);

    // 通过userId查找用户所有收藏
    public Task<List<Post>> FindCollections(int userId);

    // 点赞帖子
    public Task<int> LikePost(int postId);

    // 取消点赞帖子
    public Task<int> UnLikePost(int postId);

    // 收藏帖子
    public Task<int> CollectPost(int postId);

    // 取消收藏帖子
    public Task<int> UnCollectPost(int postId);

    // 打赏帖子
    public Task<int> RewardPost(int postId, int rewardNum, int rewardType);

    // 添加标签
    public Task<int> AddTag(int postId, string tagName);

    // 删除标签
    public Task<int> DeleteTag(int postId, string tagName);

    // 查找所有帖子
    public Task<List<Post>> FindAllPosts();

    // 模糊查找帖子
    public Task<List<Post>> FindPostsByKeys(params string[] keys);

    // 推荐自己的帖子
    public Task<int> RecommendMyPost(string postName, string reason);
}