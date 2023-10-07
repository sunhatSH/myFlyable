using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Repositories.Entities;
using Flyable.Services.IServices;

namespace Flyable.Services.Services;

public class NovelService : INovelService
{
    public NovelService(INovelAccess novelAccess)
    {
        NovelAccess = novelAccess;
    }

    public INovelAccess NovelAccess { get; set; }

    public async Task<Novel> CreateNovel(Novel novel)
    {
        return null;
    }

    public async Task<Novel> UpdateNovelInfo(Novel novel)
    {
        return null;
    }

    public async Task<int> DeleteNovel(int novelId)
    {
        return 0;
    }

    public async Task<List<Novel>> FindNovelsByNovelName(string novelName)
    {
        return null;
    }

    public async Task<List<Novel>> FindNovelsByAuthor(string author)
    {
        return null;
    }

    public async Task<List<Novel>> FindNovelsByTag(string tag)
    {
        return null;
    }

    public async Task<List<Novel>> FindCollections(int userId)
    {
        return null;
    }

    public async Task<int> AddChapter(Chapter chapter)
    {
        return 0;
    }

    public async Task<int> DeleteChapter(int chapterId)
    {
        return 0;
    }

    public async Task<int> UpdateChapter(Chapter chapter)
    {
        return 0;
    }

    public async Task<int> CollectNovel(int novelId)
    {
        return 0;
    }

    public async Task<int> UnCollectNovel(int novelId)
    {
        return 0;
    }

    public async Task<int> LikeNovel(int novelId)
    {
        return 0;
    }

    public async Task<int> UnLikeNovel(int novelId)
    {
        return 0;
    }

    public async Task<int> LikeChapter(int chapterId)
    {
        return 0;
    }

    public async Task<int> UnLikeChapter(int chapterId)
    {
        return 0;
    }

    public async Task<int> RewardNovel(int chapterId, int rewardNum, int rewardType)
    {
        return 0;
    }

    public async Task<int> RewardChapter(int chapterId, int rewardNum)
    {
        return 0;
    }

    public async Task<int> AddTag(int novelId, string tagName)
    {
        return 0;
    }

    public async Task<int> DeleteTag(int novelId, string tagName)
    {
        return 0;
    }

    public async Task<List<Novel>> FindNovelsByKeys(params string[] keys)
    {
        return null;
    }

    public async Task<int> RecommendMyNovel(string novelName, string reason)
    {
        return 0;
    }

    public async Task<List<Novel>> FindCollections(string collection)
    {
        return null;
    }

    public async Task<int> AddComment(ChapterComment comment)
    {
        return 0;
    }

    public async Task<int> DeleteComment(int commentId)
    {
        return 0;
    }

    public async Task<ChapterComment> UpdateComment(ChapterComment comment)
    {
        return null;
    }

    public async Task<int> RewardNovel(int chapterId, int rewardNum)
    {
        return 0;
    }

    public async Task<int> RewardComment(int commentId, int rewardNum)
    {
        return 0;
    }
}