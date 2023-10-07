using Flyable.Repositories.Entities;

namespace Flyable.Services.IServices;

public interface INovelService
{
    public Task<Novel> CreateNovel(Novel novel);
    public Task<Novel> UpdateNovelInfo(Novel novel);
    public Task<int> DeleteNovel(int novelId);
    public Task<List<Novel>> FindNovelsByNovelName(string novelName);
    public Task<List<Novel>> FindNovelsByAuthor(string author);
    public Task<List<Novel>> FindNovelsByTag(string tag);
    public Task<List<Novel>> FindCollections(int userId);
    public Task<int> AddChapter(Chapter chapter);
    public Task<int> DeleteChapter(int chapterId);
    public Task<int> UpdateChapter(Chapter chapter);

    public Task<int> CollectNovel(int novelId);
    public Task<int> UnCollectNovel(int novelId);
    public Task<int> LikeNovel(int novelId);
    public Task<int> UnLikeNovel(int novelId);
    public Task<int> LikeChapter(int chapterId);
    public Task<int> UnLikeChapter(int chapterId);

    public Task<int> RewardNovel(int chapterId, int rewardNum, int rewardType);
    public Task<int> RewardChapter(int chapterId, int rewardNum);
    public Task<int> AddTag(int novelId, string tagName);
    public Task<int> DeleteTag(int novelId, string tagName);

    // 模糊查找小说
    public Task<List<Novel>> FindNovelsByKeys(params string[] keys);

    // 推荐自己的小说
    public Task<int> RecommendMyNovel(string novelName, string reason);
}