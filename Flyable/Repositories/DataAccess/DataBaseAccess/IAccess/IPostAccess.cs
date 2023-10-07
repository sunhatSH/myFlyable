using Flyable.Repositories.Entities;

namespace Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;

public interface IPostAccess
{
    // 增加帖子
    public Task<int> AddPost(Post post);

    // 删除帖子
    public Task<int> DeletePost(int postId);

    // 修改帖子
    public Task<int> UpdatePost(Post post);

    // 查找帖子
    public Task<Post> FindPost(int postId);

    // 查找用户的所有帖子
    public Task<List<Post>?> FindPostsByUserId(int userId);

    // 按帖子标签查找帖子
    public Task<List<Post>> FindPostsByTag(PostTag tag);

    // 通过作者名查找帖子
    public Task<List<Post>> FindPostsByAuthor(string authorName);

    // 通过userId查找用户所有收藏
    public Task<List<Post>> FindCollections(int userId);

    // 查找所有帖子
    public Task<List<Post>?> FindAllPagedPosts(int pageNum, int pageSize);

    // 模糊查找帖子
    public Task<List<Post>?> FindPostsByKeys(params string[] keys);
}