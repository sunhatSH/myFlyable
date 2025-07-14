using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Flyable.Entities;
using Flyable.Dtos;
using Flyable.StatusCode;
using Flyable.Wraps;
using System.Text.Json;
using Flyable.Models;

namespace Flyable.Actions;

public class NovelAction(
    FlyableUserContext context,
    IDistributedCache cache)
{
    /// <summary>
    /// 获取小说列表
    /// </summary>
    public async Task<PagedResultDto<NovelListItemDto>> GetNovelListAsync(NovelQueryDto query)
    {
        var queryable = context.Novels
            .Include(n => n.LikeUsers)
            .Include(n => n.CollectionUsers)
            .Include(n => n.NovelComments)
            .Include(n => n.NovelRatings)
            .Where(n => n.NovelStatus == 0);

        // Apply search filters
        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            queryable = queryable.Where(n =>
                (n.NovelName != null && n.NovelName.Contains(query.Keyword)) ||
                (n.ShortDescription != null && n.ShortDescription.Contains(query.Keyword)) ||
                (n.AuthorName != null && n.AuthorName.Contains(query.Keyword)));
        }

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            var categoryType = ParseNovelType(query.Category);
            queryable = queryable.Where(n => n.NovelType == categoryType);
        }

        // Apply sorting
        queryable = query.SortBy?.ToLower() switch
        {
            "hot" => query.IsDescending ? queryable.OrderByDescending(n => n.Hot) : queryable.OrderBy(n => n.Hot),
            "score" => query.IsDescending ? queryable.OrderByDescending(n => n.Score) : queryable.OrderBy(n => n.Score),
            "latest" => query.IsDescending ? queryable.OrderByDescending(n => n.LastAlterTime) : queryable.OrderBy(n => n.LastAlterTime),
            "collects" => query.IsDescending ? queryable.OrderByDescending(n => n.CollectNums) : queryable.OrderBy(n => n.CollectNums),
            "likes" => query.IsDescending ? queryable.OrderByDescending(n => n.LikeNums) : queryable.OrderBy(n => n.LikeNums),
            _ => queryable.OrderByDescending(n => n.Hot) // Default sort by hot
        };

        var total = await queryable.CountAsync();
        var novels = await queryable
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var result = novels.Select(n => new NovelListItemDto
        {
            Id = n.NovelId,
            Title = n.NovelName ?? "",
            AuthorName = n.AuthorName,
            Cover = n.Cover,
            ShortDescription = n.ShortDescription,
            Score = n.Score,
            CollectNums = n.CollectNums,
            IsFinished = n.IsFinished,
            LastAlterTime = n.LastAlterTime,
            LikeCount = n.LikeNums,
            CommentCount = n.NovelComments?.Count(c => c.CommentStatus == 0) ?? 0,
            RatingCount = n.NovelRatings?.Count ?? 0,
            Hot = n.Hot
        }).ToList();

        return new PagedResultDto<NovelListItemDto>
        {
            Data = result,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    /// <summary>
    /// 根据ID获取小说详情
    /// </summary>
    public async Task<Novel?> GetNovelByIdAsync(int id)
    {
        // 先从缓存获取
        var cacheKey = $"novel_{id}";
        var cached = await cache.GetStringAsync(cacheKey);

        if (cached != null)
            return JsonSerializer.Deserialize<Novel>(cached);

        var novel = await context.Novels
            .Include(n => n.NovelComments)
            .FirstOrDefaultAsync(n => n.NovelId == id && n.NovelStatus == 0);

        if (novel != null)
        {
            // 缓存30分钟
            await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(novel),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) });
        }

        return novel;
    }

    /// <summary>
    /// 创建小说
    /// </summary>
    public async Task<Novel> CreateNovelAsync(CreateNovelDto dto)
    {
        // 确保用户存在，如果不存在则创建默认用户
        await EnsureUserExistsAsync(dto.AuthorId, "匿名用户");

        var novel = new Novel
        {
            NovelName = dto.Title,
            ShortDescription = dto.Description,
            DetailedDescription = dto.Description,
            Cover = dto.CoverImage,
            NovelType = ParseNovelType(dto.Category),
            UserId = dto.AuthorId,
            AuthorName = await GetAuthorNameAsync(dto.AuthorId),
            NovelStatus = 0, // 正常状态
            CreatedTime = DateTime.Now,
            LastAlterTime = DateTime.Now,
            LastModifyTime = DateTime.Now,
            IsFinished = false,
            IsAllowComment = true,
            IsOpenChatRoom = false,
            IsEditorRecommend = false,
            IsOriginal = true,
            IsShowOnHomePage = false,
            NovelBackground = 3, // 其他
            NovelOrientation = 3, // 无CP
            Score = 0,
            ScorePeopleNum = 0,
            FavoredNums = 0,
            LikeNums = 0,
            CollectNums = 0,
            WordCount = 0,
            TotalClicks = 0,
            TotalFeather = 0,
            TotalColorStone = 0,
            LastModifyAdminId = 0,
            ReportNums = 0
        };

        context.Novels.Add(novel);
        await context.SaveChangesAsync();

        // 清除相关缓存
        await cache.RemoveAsync("novel_list");

        return novel;
    }

    /// <summary>
    /// 更新小说信息
    /// </summary>
    public async Task<Novel> UpdateNovelAsync(int id, UpdateNovelDto dto)
    {
        var novel = await context.Novels.Include(n => n.LikeUsers).Include(n => n.NovelComments).Include(n => n.CollectionUsers).FirstOrDefaultAsync(n => n.NovelId == id);
        if (novel == null || novel.NovelStatus != 0)
            throw new ArgumentException("小说不存在");
        if (!string.IsNullOrEmpty(dto.Title))
        {
            novel.LastName = novel.NovelName; // 保存旧名称
            novel.NovelName = dto.Title;
        }
        if (!string.IsNullOrEmpty(dto.Description))
        {
            novel.ShortDescription = dto.Description;
            novel.DetailedDescription = dto.Description;
        }
        if (!string.IsNullOrEmpty(dto.CoverImage))
            novel.Cover = dto.CoverImage;
        if (!string.IsNullOrEmpty(dto.Category))
            novel.NovelType = ParseNovelType(dto.Category);
        if (!string.IsNullOrEmpty(dto.Copywriting))
            novel.Copywriting = dto.Copywriting;
        if (!string.IsNullOrEmpty(dto.SellingPoint))
            novel.SellingPoint = dto.SellingPoint;
        if (!string.IsNullOrEmpty(dto.Recommendation))
            novel.Recommendation = dto.Recommendation;
        novel.LastAlterTime = DateTime.Now;
        novel.LastModifyTime = DateTime.Now;
        await context.SaveChangesAsync();
        await RecalculateAndSaveHotAsync(novel);
        // 清除缓存
        await cache.RemoveAsync($"novel_{id}");
        await cache.RemoveAsync("novel_list");
        return novel;
    }

    /// <summary>
    /// 删除小说 (软删除)
    /// </summary>
    public async Task<bool> DeleteNovelAsync(int id)
    {
        var novel = await context.Novels.FindAsync(id);
        if (novel == null || novel.NovelStatus != 0)
            return false;

        novel.NovelStatus = 1; // 软删除
        novel.LastModifyTime = DateTime.Now;
        await context.SaveChangesAsync();

        // 清除缓存
        await cache.RemoveAsync($"novel_{id}");
        await cache.RemoveAsync("novel_list");

        return true;
    }

    /// <summary>
    /// 添加章节
    /// </summary>
    public async Task<Chapter> AddChapterAsync(int novelId, CreateChapterDto dto)
    {
        var novel = await context.Novels.Include(n => n.LikeUsers).Include(n => n.NovelComments).Include(n => n.CollectionUsers).FirstOrDefaultAsync(n => n.NovelId == novelId);
        if (novel == null || novel.NovelStatus != 0)
            throw new ArgumentException("小说不存在");
        var chapter = new Chapter
        {
            ChapterName = dto.Title,
            Content = dto.Content,
            NovelId = novelId,
            ChapterOrder = dto.ChapterNumber,
            WordCount = dto.Content?.Length ?? 0,
            ViewCount = 0,
            LikeCount = 0,
            CreateTime = DateTime.Now,
            LastAlterTime = DateTime.Now,
            ChapterStatus = 0 // 正常状态
        };
        context.Chapters.Add(chapter);
        // 更新小说信息
        novel.LastAlterTime = DateTime.Now;
        novel.LastModifyTime = DateTime.Now;
        novel.WordCount += chapter.WordCount;
        await context.SaveChangesAsync();
        await RecalculateAndSaveHotAsync(novel);
        // 清除相关缓存
        await cache.RemoveAsync($"novel_{novelId}");
        await cache.RemoveAsync($"chapters_{novelId}");
        return chapter;
    }

    /// <summary>
    /// 获取小说章节列表
    /// </summary>
    public async Task<List<object>> GetChaptersByNovelIdAsync(int novelId)
    {
        var cacheKey = $"chapters_{novelId}";
        var cached = await cache.GetStringAsync(cacheKey);

        if (cached != null)
            return JsonSerializer.Deserialize<List<object>>(cached)!;

        var chapters = await context.Chapters
            .Where(c => c.NovelId == novelId && c.ChapterStatus == 0)
            .OrderBy(c => c.ChapterOrder)
            .Select(c => new
            {
                Id = c.ChapterId,
                Title = c.ChapterName,
                ChapterNumber = c.ChapterOrder,
                c.WordCount,
                c.ViewCount,
                c.LikeCount,
                CommentCount = c.CommentCount,
                c.CreateTime,
                c.LastAlterTime
            })
            .ToListAsync();

        if (chapters.Any())
        {
            await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(chapters),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) });
        }

        return chapters.Cast<object>().ToList();
    }

    /// <summary>
    /// 获取指定章节
    /// </summary>
    public async Task<Chapter?> GetChapterAsync(int novelId, int chapterId)
    {
        return await context.Chapters
            .FirstOrDefaultAsync(c => c.NovelId == novelId && c.ChapterId == chapterId && c.ChapterStatus == 0);
    }

    /// <summary>
    /// 添加评论
    /// </summary>
    public async Task<NovelComment> AddCommentAsync(int novelId, CreateCommentDto dto)
    {
        var novel = await context.Novels.Include(n => n.LikeUsers).Include(n => n.NovelComments).Include(n => n.CollectionUsers).FirstOrDefaultAsync(n => n.NovelId == novelId);
        if (novel == null || novel.NovelStatus != 0)
            throw new ArgumentException("小说不存在");
        if (!novel.IsAllowComment)
            throw new ArgumentException("该小说不允许评论");
        var comment = new NovelComment
        {
            Content = dto.Content,
            UserId = dto.UserId,
            PublishTime = DateTime.Now,
            CommentStatus = 0, // 正常状态
            ReplyCount = 0,
            IsTop = false,
            IsRecommend = false,
            IsHot = false,
            NovelId = novelId // 显式赋值，修复保存报错
        };
        context.NovelComments.Add(comment);
        await context.SaveChangesAsync();
        await RecalculateAndSaveHotAsync(novel);
        // 清除评论缓存
        await cache.RemoveAsync($"comments_{novelId}");
        return comment;
    }

    /// <summary>
    /// 获取小说评论列表
    /// </summary>
    public async Task<object> GetCommentsByNovelIdAsync(int novelId, int page, int pageSize)
    {
        var comments = await context.NovelComments
            .Where(c => c.BelongsNovel != null && c.BelongsNovel.NovelId == novelId && c.CommentStatus == 0)
            .OrderByDescending(c => c.PublishTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Join(context.Users, c => c.UserId, u => u.UserId, (c, u) => new
            {
                Id = c.NovelCommentId,
                c.Content,
                c.UserId,
                Username = u.Username,
                c.PublishTime,
                LikeCount = c.LikeCount,
                c.ReplyCount,
                c.IsTop,
                c.IsRecommend,
                c.IsHot
            })
            .ToListAsync();

        var total = await context.NovelComments
            .CountAsync(c => c.BelongsNovel != null && c.BelongsNovel.NovelId == novelId && c.CommentStatus == 0);

        return new
        {
            Data = comments,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// 搜索小说
    /// </summary>
    public async Task<List<Novel>> SearchNovelsAsync(string keyword, string? category = null)
    {
        var query = context.Novels.Where(n => n.NovelStatus == 0).AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(n => (n.NovelName != null && n.NovelName.Contains(keyword)) ||
                                   (n.ShortDescription != null && n.ShortDescription.Contains(keyword)) ||
                                   (n.AuthorName != null && n.AuthorName.Contains(keyword)) ||
                                   (n.LastName != null && n.LastName.Contains(keyword)));
        }

        if (!string.IsNullOrEmpty(category))
        {
            var novelType = ParseNovelType(category);
            query = query.Where(n => n.NovelType == novelType);
        }

        return await query.OrderByDescending(n => n.LastAlterTime).ToListAsync();
    }

    // 辅助方法
    private static string GetNovelTypeString(int novelType)
    {
        return novelType switch
        {
            0 => "网游",
            1 => "玄幻",
            2 => "都市",
            3 => "科幻",
            4 => "灵异",
            5 => "历史",
            6 => "穿越",
            7 => "重生",
            8 => "竞技",
            _ => "其他"
        };
    }

    private static int ParseNovelType(string category)
    {
        return category switch
        {
            "网游" => 0,
            "玄幻" => 1,
            "都市" => 2,
            "科幻" => 3,
            "灵异" => 4,
            "历史" => 5,
            "穿越" => 6,
            "重生" => 7,
            "竞技" => 8,
            _ => 9 // 其他
        };
    }

    private async Task<string> GetAuthorNameAsync(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        return user?.Username ?? "未知作者";
    }

    /// <summary>
    /// 获取小说榜单
    /// </summary>
    public async Task<object> GetNovelRankingsAsync(string type, int limit)
    {
        var query = context.Novels.Where(n => n.NovelStatus == 0).AsQueryable();

        query = type.ToLower() switch
        {
            "hot" => query.Include(n => n.LikeUsers)
                .Include(n => n.NovelComments)
                .Include(n => n.CollectionUsers)
                .OrderByDescending(n => n.Hot),
            "new" => query.OrderByDescending(n => n.CreatedTime),
            "completed" => query.Where(n => n.IsFinished).OrderByDescending(n => n.LastAlterTime),
            "score" => query.OrderByDescending(n => n.Score),
            "recommend" => query.Where(n => n.IsEditorRecommend).OrderByDescending(n => n.LastAlterTime),
            "favorite" => query.OrderByDescending(n => n.CollectNums),
            _ => query.OrderByDescending(n => n.Hot)
        };

        var novels = await query
            .Take(limit)
            .Select(n => new
            {
                Id = n.NovelId,
                Title = n.NovelName,
                AuthorName = n.AuthorName,
                Category = GetNovelTypeString(n.NovelType),
                Cover = n.Cover,
                Score = n.Score,
                TotalClicks = n.TotalClicks,
                CollectNums = n.CollectNums,
                WordCount = n.WordCount,
                IsFinished = n.IsFinished,
                UpdatedTime = n.LastAlterTime,
                LikeCount = n.LikeUsers.Count,
                CommentCount = n.NovelComments.Count(c => c.CommentStatus == 0),
                ShortDescription = n.ShortDescription,
                Hot = n.Hot
            })
            .ToListAsync();

        return new { Type = type, Data = novels };
    }

    /// <summary>
    /// 获取推荐小说
    /// </summary>
    public async Task<List<object>> GetRecommendationsAsync(int limit)
    {
        var novels = await context.Novels
            .Where(n => n.NovelStatus == 0 && (n.IsEditorRecommend || n.Score >= 4.0))
            .OrderByDescending(n => n.Score)
            .ThenByDescending(n => n.TotalClicks)
            .Include(n => n.LikeUsers)
            .Include(n => n.NovelComments)
            .Include(n => n.CollectionUsers)
            .Take(limit)
            .Select(n => new
            {
                Id = n.NovelId,
                Title = n.NovelName,
                AuthorName = n.AuthorName,
                Category = GetNovelTypeString(n.NovelType),
                Cover = n.Cover,
                Score = n.Score,
                Description = n.ShortDescription,
                TotalClicks = n.TotalClicks,
                WordCount = n.WordCount,
                IsFinished = n.IsFinished,
                IsEditorRecommend = n.IsEditorRecommend,
                EditorRecommendReason = n.EditorRecommendReason,
                LikeCount = n.LikeUsers.Count,
                CommentCount = n.NovelComments.Count(c => c.CommentStatus == 0),
                CollectNums = n.CollectNums,
                Hot = n.Hot
            })
            .ToListAsync();

        return novels.Cast<object>().ToList();
    }

    /// <summary>
    /// 获取小说详情（包含章节列表），并返回当前用户是否已点赞和评分
    /// </summary>
    public async Task<object?> GetNovelDetailAsync(int id, int? userId = null)
    {
        var novel = await context.Novels
            .Include(n => n.LikeUsers)
            .Include(n => n.CollectionUsers)
            .Include(n => n.NovelComments)
            .Where(n => n.NovelId == id && n.NovelStatus == 0)
            .Select(n => new
            {
                Id = n.NovelId,
                Title = n.NovelName,
                AuthorName = n.AuthorName,
                Category = GetNovelTypeString(n.NovelType),
                Cover = n.Cover,
                ShortDescription = n.ShortDescription,
                DetailedDescription = n.DetailedDescription,
                Score = n.Score,
                TotalClicks = n.TotalClicks,
                CollectNums = n.CollectNums,
                WordCount = n.WordCount,
                IsFinished = n.IsFinished,
                IsEditorRecommend = n.IsEditorRecommend,
                EditorRecommendReason = n.EditorRecommendReason,
                CreatedTime = n.CreatedTime,
                LastAlterTime = n.LastAlterTime,
                NovelTags = n.NovelTags,
                // LikeNums 实时统计
                LikeNums = context.NovelLikes.Count(l => l.NovelId == id),
                // IsLiked 直接查 NovelLikes 表
                IsLiked = userId != null && context.NovelLikes.Any(l => l.NovelId == id && l.UserId == userId),
                IsCollected = userId != null && n.CollectionUsers.Any(u => u.UserId == userId),
                Hot = n.Hot
            })
            .FirstOrDefaultAsync();

        if (novel == null) return null;

        // 获取章节列表
        var chapters = await context.Chapters
            .Where(c => c.NovelId == id && c.ChapterStatus == 0)
            .OrderBy(c => c.ChapterOrder)
            .Select(c => new
            {
                Id = c.ChapterId,
                Title = c.ChapterName,
                ChapterOrder = c.ChapterOrder,
                WordCount = c.WordCount,
                CreateTime = c.CreateTime
            })
            .ToListAsync();

        // 查询当前用户评分
        int? userRating = null;
        if (userId != null)
        {
            var rating = await context.NovelRatings.FirstOrDefaultAsync(r => r.NovelId == id && r.UserId == userId);
            if (rating != null) userRating = rating.Rating;
        }
        // 评分人数用 NovelRatings.Count
        int scorePeopleNum = await context.NovelRatings.CountAsync(r => r.NovelId == id);

        return new
        {
            Novel = novel,
            Chapters = chapters,
            UserRating = userRating,
            ScorePeopleNum = scorePeopleNum
        };
    }

    /// <summary>
    /// 为小说评分，防止重复评分
    /// </summary>
    public async Task<object> RateNovelAsync(int novelId, RateNovelDto dto)
    {
        var novel = await context.Novels.FindAsync(novelId);
        if (novel == null || novel.NovelStatus != 0)
            throw new ArgumentException("小说不存在");

        // 检查是否已评分
        var exist = await context.NovelRatings.FirstOrDefaultAsync(r => r.NovelId == novelId && r.UserId == dto.UserId);
        if (exist != null)
            throw new InvalidOperationException("你已经给本书评分过了");

        // 写入评分记录
        context.NovelRatings.Add(new Entities.NovelRating
        {
            NovelId = novelId,
            UserId = dto.UserId,
            Rating = dto.Rating,
            RatedAt = DateTime.Now
        });

        // 更新平均分
        var totalScore = novel.Score * novel.ScorePeopleNum + dto.Rating;
        novel.ScorePeopleNum += 1;
        novel.Score = Math.Round(totalScore / novel.ScorePeopleNum, 1);

        await context.SaveChangesAsync();

        return new
        {
            NovelId = novelId,
            NewScore = novel.Score,
            TotalRaters = novel.ScorePeopleNum
        };
    }

    /// <summary>
    /// 确保用户存在
    /// </summary>
    private async Task EnsureUserExistsAsync(int userId, string username)
    {
        var userExists = await context.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            var user = new User
            {
                UserId = userId,
                Username = username,
                PwdToken = "default_password",
                Email = $"{username}@example.com",
                Role = UserRole.Reader,
                RegisterTime = DateTime.Now,
                LastLoginTime = DateTime.Now,
                Ip = "127.0.0.1",
                Status = UserStatusCode.Normal,
                Level = UserLevel.FeatherStart,
                CollectionNovels = new List<Novel>(),
                CollectionPosts = new List<Post>(),
                Follows = new List<User>(),
                Followed = new List<User>(),
                ColorStone = 1000,
                Feather = 100,
                ReportCount = 0,
                LastModifyTime = DateTime.Now,
                LastModifyAdmin = null,
                SelfIntroduction = "默认用户",
                IsVip = false,
                HeadLink = "default.png"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
    }

    // 收藏/取消收藏小说
    public async Task<bool> ToggleCollectNovelAsync(int novelId, int userId)
    {
        var novel = await context.Novels.Include(n => n.CollectionUsers).Include(n => n.LikeUsers).Include(n => n.NovelComments).FirstOrDefaultAsync(n => n.NovelId == novelId);
        if (novel is not { NovelStatus: 0 })
            throw new ArgumentException("小说不存在");
        var user = await context.Users.Include(u => u.CollectionNovels).FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
            throw new ArgumentException("用户不存在");
        var collected = novel.CollectionUsers.Any(u => u.UserId == userId);
        if (collected)
        {
            novel.CollectionUsers.RemoveAll(u => u.UserId == userId);
            user.CollectionNovels.RemoveAll(n => n.NovelId == novelId);
        }
        else
        {
            novel.CollectionUsers.Add(user);
            user.CollectionNovels.Add(novel);
        }
        novel.CollectNums = novel.CollectionUsers.Count;
        await context.SaveChangesAsync();
        await RecalculateAndSaveHotAsync(novel);
        return !collected; // true=收藏，false=取消
    }

    public async Task<bool> RecordNovelViewAsync(int novelId, int? userId, string ip)
    {
        var cacheKey = $"novel_view_{novelId}_{userId ?? 0}_{ip}";
        var exists = await cache.GetStringAsync(cacheKey);
        if (exists != null) return false; // 30分钟内已统计
        var novel = await context.Novels.Include(n => n.LikeUsers).Include(n => n.NovelComments).Include(n => n.CollectionUsers).FirstOrDefaultAsync(n => n.NovelId == novelId);
        if (novel == null) return false;
        novel.TotalClicks += 1;
        await context.SaveChangesAsync();
        await RecalculateAndSaveHotAsync(novel);
        await cache.SetStringAsync(cacheKey, "1", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });
        return true;
    }

    public async Task SaveNovelAsync(Novel novel)
    {
        context.Novels.Update(novel);
        await context.SaveChangesAsync();
    }

    public async Task<bool> EditNovelCommentAsync(int commentId, int userId, string newContent)
    {
        var comment = await context.NovelComments.FindAsync(commentId);
        if (comment == null) return false;
        if (comment.UserId != userId) return false;
        comment.Content = newContent;
        comment.IsEdited = true;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteNovelCommentAsync(int commentId, int userId)
    {
        var comment = await context.NovelComments.FindAsync(commentId);
        if (comment == null) return false;
        // 允许评论作者或小说作者删除
        var novel = await context.Novels.FindAsync(comment.NovelId);
        if (comment.UserId != userId && (novel == null || novel.UserId != userId)) return false;
        context.NovelComments.Remove(comment);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// 获取每本小说的最新一章，按更新时间倒序（兼容EF Core）
    /// </summary>
    public async Task<List<object>> GetLatestNovelUpdatesAsync(int limit = 10)
    {
        var novels = await context.Novels
            .Where(n => n.NovelStatus == 0)
            .Select(n => new { n.NovelId, n.NovelName, n.Cover, n.AuthorName })
            .ToListAsync();

        var latestList = new List<dynamic>();
        foreach (var n in novels)
        {
            var chapter = await context.Chapters
                .Where(c => c.NovelId == n.NovelId && c.ChapterStatus == 0)
                .OrderByDescending(c => c.LastAlterTime)
                .FirstOrDefaultAsync();
            if (chapter != null)
            {
                latestList.Add(new
                {
                    NovelId = n.NovelId,
                    NovelTitle = n.NovelName,
                    NovelCover = n.Cover,
                    AuthorName = n.AuthorName,
                    ChapterId = chapter.ChapterId,
                    ChapterTitle = chapter.ChapterName,
                    ChapterOrder = chapter.ChapterOrder,
                    ChapterUpdateTime = chapter.LastAlterTime,
                    ChapterCreateTime = chapter.CreateTime,
                    WordCount = chapter.WordCount
                });
            }
        }
        return latestList.OrderByDescending(x => x.ChapterUpdateTime).Take(limit).ToList<object>();
    }

    /// <summary>
    /// 从txt文件解析并导入小说和章节，如果存在同名小说则阻止上传，作者使用当前用户sunhat
    /// </summary>
    public async Task<bool> ImportNovelFromTxtAsync(string txtContent)
    {
        // 解析txt内容，提取小说元信息和章节
        var lines = txtContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        string title = null;
        string author = "sunhat"; // 使用当前用户sunhat作为作者
        var chapters = new List<(string Title, string Content)>();

        string currentChapterTitle = null;
        var currentChapterContent = new List<string>();

        foreach (var line in lines)
        {
            if (line.StartsWith("标题："))
            {
                title = line.Substring(3).Trim();
            }
            else if (line.StartsWith("章节"))
            {
                // 如果已有章节，保存当前章节
                if (currentChapterTitle != null)
                {
                    chapters.Add((currentChapterTitle, string.Join("\n", currentChapterContent)));
                    currentChapterContent.Clear();
                }
                // 解析新章节
                var parts = line.Split(new[] { ':', '：' }, 2);
                if (parts.Length > 1)
                {
                    currentChapterTitle = parts[1].Trim();
                }
            }
            else if (currentChapterTitle != null)
            {
                currentChapterContent.Add(line);
            }
        }

        // 保存最后一章
        if (currentChapterTitle != null)
        {
            chapters.Add((currentChapterTitle, string.Join("\n", currentChapterContent)));
        }

        // 检查是否存在同名小说
        var existingNovel = await context.Novels.FirstOrDefaultAsync(n => n.NovelName == title);
        if (existingNovel != null)
        {
            return false; // 阻止上传
        }

        // 创建小说
        var novel = new Novel
        {
            NovelName = title,
            AuthorName = author,
            NovelStatus = 0, // 正常状态
            CreatedTime = DateTime.Now,
            LastAlterTime = DateTime.Now
        };
        context.Novels.Add(novel);
        await context.SaveChangesAsync();

        // 创建章节
        foreach (var chapter in chapters)
        {
            var chapterEntity = new Chapter
            {
                NovelId = novel.NovelId,
                ChapterName = chapter.Title,
                Content = chapter.Content,
                ChapterStatus = 0, // 正常状态
                CreateTime = DateTime.Now,
                LastAlterTime = DateTime.Now
            };
            context.Chapters.Add(chapterEntity);
        }
        await context.SaveChangesAsync();

        return true;
    }

    private async Task RecalculateAndSaveHotAsync(Novel novel)
    {
        novel.Hot = Novel.CalculateHot(novel);
        context.Novels.Update(novel);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ToggleLikeNovelAsync(int novelId, int userId)
    {
        var like = await context.NovelLikes.FirstOrDefaultAsync(l => l.NovelId == novelId && l.UserId == userId);
        if (like != null)
        {
            context.NovelLikes.Remove(like);
            var novel = await context.Novels.FindAsync(novelId);
            if (novel != null) novel.LikeNums = Math.Max(0, novel.LikeNums - 1);
            await context.SaveChangesAsync();
            await RecalculateAndSaveHotAsync(novel);
            return false; // 取消点赞
        }
        else
        {
            context.NovelLikes.Add(new NovelLike { NovelId = novelId, UserId = userId, CreateTime = DateTime.Now });
            var novel = await context.Novels.FindAsync(novelId);
            if (novel != null) novel.LikeNums += 1;
            await context.SaveChangesAsync();
            await RecalculateAndSaveHotAsync(novel);
            return true; // 点赞
        }
    }

    public async Task<int> GetNovelLikeCountAsync(int novelId)
    {
        return await context.NovelLikes.CountAsync(l => l.NovelId == novelId);
    }
}