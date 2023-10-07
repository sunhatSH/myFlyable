using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flyable.Repositories.DataAccess.DataBaseAccess.Access;

public class CommentAccess : ICommentAccess
{
    public CommentAccess(FlyableUserContext context)
    {
        _context = context;
    }

    private readonly FlyableUserContext _context;

    public async Task<PostComment?> GetPostCommentAsync(int commentId)
    {
        return await _context.PostComments!
            .Where(comment => comment.PostCommentId == commentId)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// 按照指定方式排序
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="pageNum"></param>
    /// <param name="pageSize"></param>
    /// <param name="orderBy">
    /// <code>
    /// 1：按照点赞数降序
    /// 2: 按照回复数降序
    /// 3: 按照时间降序
    /// </code>
    /// </param>
    /// <returns></returns>
    public async Task<List<PostComment>> GetPostCommentsAsync(int postId, int pageNum, int pageSize, int orderBy)
    {
        return orderBy switch
        {
            1 => await _context.Posts!
                .Where(post => post.PostId == postId)
                .Include(post => post.PostComments)
                .SelectMany(post => post.PostComments!
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize))
                .OrderByDescending(comment => comment.LikeCount)
                .ToListAsync(),
            2 => await _context.Posts!
                .Where(post => post.PostId == postId)
                .Include(post => post.PostComments)
                .SelectMany(post => post.PostComments!
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize))
                .OrderByDescending(comment => comment.ReplyCount)
                .ToListAsync(),
            3 => await _context.Posts!
                .Where(post => post.PostId == postId)
                .Include(post => post.PostComments)
                .SelectMany(post => post.PostComments!
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize))
                .OrderByDescending(comment => comment.PublishTime)
                .ToListAsync(),
            var _ => throw new Exception("未知排序方式")
        };
    }

    public async Task<int> DeletePostCommentAsync(int commentId)
    {
        return await _context.PostComments!
            .Where(comment => comment.PostCommentId == commentId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(comment => comment.CommentStatus, 1)
            );
    }

    public async Task<int> AddPostCommentAsync(PostComment comment)
    {
        return await _context.Posts!
            .Where(post => post.PostId == comment.BelongsPost!.PostId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(
                    post => post.PostComments,
                    post => post.PostComments!.Append(comment))
            );
    }

    public async Task<int> UpdatePostCommentAsync(int commentId, string content)
    {
        return await _context.PostComments!
            .Where(comment => comment.PostCommentId == commentId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(comment => comment.Content, content)
            );
    }

    public async Task<NovelComment?> GetNovelCommentAsync(int commentId)
    {
        return await _context.NovelComments!
            .Where(comment => comment.NovelCommentId == commentId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<NovelComment>> GetNovelCommentsAsync(int novelId, int pageNum, int pageSize)
    {
        return await _context.Novels
            .Where(novel => novel.NovelId == novelId)
            .Include(novel => novel.NovelComments)
            .SelectMany(novel => novel.NovelComments!
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize))
            .ToListAsync();
    }

    public async Task<List<NovelComment>> GetNovelCommentsAsync(int novelId, int pageNum, int pageSize, int orderBy) =>
        orderBy switch
        {
            1 => await _context.Novels
                .Where(novel => novel.NovelId == novelId)
                .Include(novel => novel.NovelComments)
                .SelectMany(novel => novel.NovelComments!
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize))
                .OrderByDescending(comment => comment.LikeCount)
                .ToListAsync(),
            2 => await _context.Novels
                .Where(novel => novel.NovelId == novelId)
                .Include(novel => novel.NovelComments)
                .SelectMany(novel => novel.NovelComments!
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize))
                .OrderByDescending(comment => comment.ReplyCount)
                .ToListAsync(),
            3 => await _context.Novels
                .Where(novel => novel.NovelId == novelId)
                .Include(novel => novel.NovelComments)
                .SelectMany(novel => novel.NovelComments!
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize))
                .OrderByDescending(comment => comment.PublishTime)
                .ToListAsync(),
            var _ => throw new Exception("未知排序方式")
        };

    public async Task<int> DeleteNovelCommentAsync(int commentId)
    {
        return await _context.NovelComments!
            .Where(comment => comment.NovelCommentId == commentId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(comment => comment.CommentStatus, 1)
            );
    }

    public async Task<int> AddNovelCommentAsync(NovelComment comment)
    {
        return await _context.Novels
            .Where(novel => novel.NovelId == comment.BelongsNovel!.NovelId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(
                    novel => novel.NovelComments,
                    novel => novel.NovelComments!.Append(comment))
            );
    }

    public async Task<int> UpdateNovelCommentAsync(int commentId, string content)
    {
        return await _context.NovelComments!
            .Where(comment => comment.NovelCommentId == commentId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(comment => comment.Content, content)
            );
    }

    public async Task<ChapterComment?> GetChapterCommentAsync(int commentId)
    {
        return await _context.ChapterComments!
            .Where(comment => comment.ChapterCommentId == commentId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<ChapterComment>> GetChapterCommentsAsync(int chapterId, int pageNum, int pageSize)
    {
        return await _context.Chapters!
            .Where(chapter => chapter.ChapterId == chapterId)
            .Include(chapter => chapter.ChapterComments)
            .SelectMany(chapter => chapter.ChapterComments!
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize))
            .ToListAsync();
    }

    public async Task<List<ChapterComment>> GetChapterCommentsAsync(int chapterId, int pageNum, int pageSize,
        int orderBy)
    {
        return orderBy switch
        {
            1 => await _context.Chapters!
                .Where(chapter => chapter.ChapterId == chapterId)
                .Include(chapter => chapter.ChapterComments)
                .SelectMany(chapter => chapter.ChapterComments!
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize))
                .OrderByDescending(comment => comment.LikeCount)
                .ToListAsync(),
            2 => await _context.Chapters!
                .Where(chapter => chapter.ChapterId == chapterId)
                .Include(chapter => chapter.ChapterComments)
                .SelectMany(chapter => chapter.ChapterComments!
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize))
                .OrderByDescending(comment => comment.ReplyCount)
                .ToListAsync(),
            3 => await _context.Chapters!
                .Where(chapter => chapter.ChapterId == chapterId)
                .Include(chapter => chapter.ChapterComments)
                .SelectMany(chapter => chapter.ChapterComments!
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize))
                .OrderByDescending(comment => comment.PublishTime)
                .ToListAsync(),
            var _ => throw new Exception("未知排序方式")
        };
    }

    public async Task<int> DeleteChapterCommentAsync(int commentId)
    {
        return await _context.ChapterComments!
            .Where(comment => comment.ChapterCommentId == commentId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(comment => comment.CommentStatus, 1)
            );
    }

    public async Task<int> AddChapterCommentAsync(ChapterComment comment)
    {
        return await _context.Chapters!
            .Where(chapter => chapter.ChapterId == comment.BelongsChapter!.ChapterId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(
                    chapter => chapter.ChapterComments,
                    chapter => chapter.ChapterComments!.Append(comment))
            );
    }

    public async Task<int> UpdateChapterCommentAsync(int commentId, string content)
    {
        return await _context.ChapterComments!
            .Where(comment => comment.ChapterCommentId == commentId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(comment => comment.Content, content)
            );
    }

    public async Task<PostComment?> GetPostCommentReplyAsync(int commentId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<PostComment>> GetPostCommentRepliesAsync(int commentId, int pageNum, int pageSize,
        int orderBy)
    {
        throw new NotImplementedException();
    }

    public async Task<int> DeletePostCommentReplyAsync(int commentId) =>
        throw new NotImplementedException();

    public async Task<int> AddPostCommentReplyAsync(PostComment comment)
    {
        return 0;
    }

    public async Task<int> UpdatePostCommentReplyAsync(int commentId, string content)
    {
        return 0;
    }

    public async Task<NovelComment> GetNovelCommentReplyAsync(int commentId)
    {
        return null;
    }

    public async Task<List<NovelComment>> GetNovelCommentRepliesAsync(int commentId, int pageNum, int pageSize)
    {
        return null;
    }

    public async Task<List<NovelComment>> GetNovelCommentRepliesAsync(int commentId, int pageNum, int pageSize,
        int orderBy)
    {
        return null;
    }

    public async Task<int> DeleteNovelCommentReplyAsync(int commentId)
    {
        return 0;
    }

    public async Task<int> AddNovelCommentReplyAsync(NovelComment comment)
    {
        return 0;
    }

    public async Task<int> UpdateNovelCommentReplyAsync(int commentId, string content) =>
        await _context.NovelComments!
            .Where(comment => comment.NovelCommentId == commentId)
            .ExecuteUpdateAsync(calls => calls.SetProperty(comment => comment.Content, content));

    public async Task<ChapterComment?> GetChapterCommentReplyAsync(int commentId) =>
        await _context.ChapterComments!
            .Where(comment => comment.ChapterCommentId == commentId)
            .FirstOrDefaultAsync();

    public async Task<List<ChapterComment>> GetChapterCommentRepliesAsync(int commentId, int pageNum, int pageSize) =>
        await _context.ChapterComments!
            .Where(comment => comment.ChapterCommentId == commentId)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<List<ChapterComment>> GetChapterCommentRepliesAsync(int commentId, int pageNum, int pageSize,
        int orderBy) =>
        throw new NotImplementedException();
        // orderBy switch
        // {
        //     1 => await _context.ChapterComments!
        //         .Where(comment => comment.ReplyTo == commentId)
        //         .OrderByDescending(comment => comment.LikeCount)
        //         .Skip((pageNum - 1) * pageSize)
        //         .Take(pageSize)
        //         .ToListAsync(),
        //     2 => await _context.ChapterComments!
        //         .Where(comment => comment.ReplyTo == commentId)
        //         .OrderByDescending(comment => comment.ReplyCount)
        //         .Skip((pageNum - 1) * pageSize)
        //         .Take(pageSize)
        //         .ToListAsync(),
        //     3 => await _context.ChapterComments!
        //         .Where(comment => comment.ReplyTo == commentId)
        //         .OrderByDescending(comment => comment.PublishTime)
        //         .Skip((pageNum - 1) * pageSize)
        //         .Take(pageSize)
        //         .ToListAsync(),
        //     var _ => throw new Exception("未知排序方式")
        // };


    public async Task<int> DeleteChapterCommentReplyAsync(int commentId) =>
        await _context.ChapterComments!
            .Where(comment => comment.ChapterCommentId == commentId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(comment => comment.CommentStatus, 1)
            );

    public async Task<int> AddChapterCommentReplyAsync(ChapterComment comment)
    {
        throw new NotImplementedException();
        // await _context.Chapters!
        //     .Where(chapter => chapter.ChapterId == comment.ReplyTo)
        //     .ExecuteUpdateAsync(
        //         calls => calls.SetProperty(
        //             chapter => chapter.ChapterComments,
        //             chapter => chapter.ChapterComments!.Append(comment))
        //     );
        // return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateChapterCommentReplyAsync(int commentId, string content)
    {
        throw new NotImplementedException();
        // return await _context.ChapterComments!
        //     .Where(comment => comment.ChapterCommentId == commentId && comment.IsReply)
        //     .ExecuteUpdateAsync(
        //         calls => calls.SetProperty(comment => comment.Content, content)
        //     );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="commentId" ></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public async Task<int> DislikeCommentAsync(int commentId, Type targetType)
    {
        const int userId = 1; //TODO: 从token中获取
        int updateResult;
        int deleteResult;
        if (targetType == typeof(Post))
        {
            updateResult = await _context.PostComments!
                .Where(comment => comment.PostCommentId == commentId)
                .ExecuteUpdateAsync(
                    calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount - 1)
                );
            deleteResult = await (_context.PostComments!.Where(comment => comment.PostCommentId == commentId)
                    .Include(postComment => postComment.LikeUsers)
                    .Single(comment => comment.PostCommentId == commentId)
                    .LikeUsers ?? throw new InvalidOperationException()).AsQueryable()
                .Where(user => user.UserId == userId)
                .ExecuteDeleteAsync();
        }
        else if (targetType == typeof(Novel))
        {
            updateResult = await _context.NovelComments!
                .Where(comment => comment.NovelCommentId == commentId)
                .ExecuteUpdateAsync(
                    calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount - 1)
                );
            deleteResult = await (_context.NovelComments!.Where(comment => comment.NovelCommentId == commentId)
                    .Include(novelComment => novelComment.LikeUsers)
                    .Single(comment => comment.NovelCommentId == commentId)
                    .LikeUsers ?? throw new InvalidOperationException()).AsQueryable()
                .Where(user => user.UserId == userId)
                .ExecuteDeleteAsync();
        }
        else if (targetType == typeof(Chapter))
        {
            updateResult = await _context.ChapterComments!
                .Where(comment => comment.ChapterCommentId == commentId)
                .ExecuteUpdateAsync(
                    calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount - 1)
                );
            deleteResult = await (_context.ChapterComments!.Where(comment => comment.ChapterCommentId == commentId)
                    .Include(chapterComment => chapterComment.LikeUsers)
                    .Single(comment => comment.ChapterCommentId == commentId)
                    .LikeUsers ?? throw new InvalidOperationException()).AsQueryable()
                .Where(user => user.UserId == userId)
                .ExecuteDeleteAsync();
        }
        // else if (targetType == typeof(PostComment))
        // {
        //     updateResult = await _context.PostComments!
        //         .Where(comment => comment.PostCommentId == commentId && comment.IsReply)
        //         .ExecuteUpdateAsync(
        //             calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount - 1)
        //         );
        //     deleteResult = await (_context.PostComments!.Where(comment => comment.PostCommentId == commentId && comment.IsReply)
        //             .Include(postComment => postComment.LikeUsers)
        //             .Single(comment => comment.PostCommentId == commentId && comment.IsReply)
        //             .LikeUsers ?? throw new InvalidOperationException()).AsQueryable()
        //         .Where(user => user.UserId == userId)
        //         .ExecuteDeleteAsync();
        // }
        // else if (targetType == typeof(NovelComment))
        // {
        //     updateResult = await _context.NovelComments!
        //         .Where(comment => comment.NovelCommentId == commentId && comment.IsReply)
        //         .ExecuteUpdateAsync(
        //             calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount - 1)
        //         );
        //     deleteResult = await (_context.NovelComments!.Where(comment => comment.NovelCommentId == commentId && comment.IsReply)
        //             .Include(novelComment => novelComment.LikeUsers)
        //             .Single(comment => comment.NovelCommentId == commentId && comment.IsReply)
        //             .LikeUsers ?? throw new InvalidOperationException()).AsQueryable()
        //         .Where(user => user.UserId == userId)
        //         .ExecuteDeleteAsync();
        // }
        // else if (targetType == typeof(ChapterComment))
        // {
        //     updateResult = await _context.ChapterComments!
        //         .Where(comment => comment.ChapterCommentId == commentId && comment.IsReply)
        //         .ExecuteUpdateAsync(
        //             calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount - 1)
        //         );
        //     deleteResult = await (_context.ChapterComments!.Where(comment => comment.ChapterCommentId == commentId && comment.IsReply)
        //             .Include(chapterComment => chapterComment.LikeUsers)
        //             .Single(comment => comment.ChapterCommentId == commentId && comment.IsReply)
        //             .LikeUsers ?? throw new InvalidOperationException()).AsQueryable()
        //         .Where(user => user.UserId == userId)
        //         .ExecuteDeleteAsync();
        // }
        else
        {
            throw new Exception("未知目标类型，无法点赞");
        }
        return updateResult == deleteResult ? 1 : 0;
    }

    public async Task<int> LikeCommentAsync(int commentId, Type targetType)
    {
        const int userId = 1; //TODO: 从token中获取
        int updateResult;
        int insertResult;
        if (targetType == typeof(Post))
        {
            updateResult = await _context.PostComments!
                .Where(comment => comment.PostCommentId == commentId)
                .ExecuteUpdateAsync(
                    calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount + 1)
                );
            (await _context.PostComments!
                    .Where(comment => comment.PostCommentId == commentId)
                    .Include(postComment => postComment.LikeUsers)
                    .SingleAsync(comment => comment.PostCommentId == commentId))
                    .LikeUsers.Add(_context.Users.Single(user => user.UserId == userId));
            insertResult = await _context.SaveChangesAsync();
        }
        else if (targetType == typeof(Novel))
        {
            updateResult = await _context.NovelComments!
                .Where(comment => comment.NovelCommentId == commentId)
                .ExecuteUpdateAsync(
                    calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount + 1)
                );
            (await _context.NovelComments!
                    .Where(comment => comment.NovelCommentId == commentId)
                    .Include(novelComment => novelComment.LikeUsers)
                    .SingleAsync(comment => comment.NovelCommentId == commentId))
                    .LikeUsers.Add(_context.Users.Single(user => user.UserId == userId));
            insertResult = await _context.SaveChangesAsync();
        }
        else if (targetType == typeof(Chapter))
        {
            updateResult = await _context.ChapterComments!
                .Where(comment => comment.ChapterCommentId == commentId)
                .ExecuteUpdateAsync(
                    calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount + 1)
                );
            (await _context.ChapterComments!
                    .Where(comment => comment.ChapterCommentId == commentId)
                    .Include(chapterComment => chapterComment.LikeUsers)
                    .SingleAsync(comment => comment.ChapterCommentId == commentId))
                    .LikeUsers.Add(_context.Users.Single(user => user.UserId == userId));
            insertResult = await _context.SaveChangesAsync();
        }
        // else if (targetType == typeof(PostComment))
        // {
        //     updateResult = await _context.PostComments!
        //         .Where(comment => comment.PostCommentId == commentId && comment.IsReply)
        //         .ExecuteUpdateAsync(
        //             calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount + 1)
        //         );
        //     (await _context.PostComments!
        //             .Where(comment => comment.PostCommentId == commentId && comment.IsReply)
        //             .Include(postComment => postComment.LikeUsers)
        //             .SingleAsync(comment => comment.PostCommentId == commentId && comment.IsReply))
        //             .LikeUsers.Add(_context.Users.Single(user => user.UserId == userId));
        //     insertResult = await _context.SaveChangesAsync();
        // }
        // else if (targetType == typeof(NovelComment))
        // {
        //     updateResult = await _context.NovelComments!
        //         .Where(comment => comment.NovelCommentId == commentId && comment.IsReply)
        //         .ExecuteUpdateAsync(
        //             calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount + 1)
        //         );
        //     (await _context.NovelComments!
        //             .Where(comment => comment.NovelCommentId == commentId && comment.IsReply)
        //             .Include(novelComment => novelComment.LikeUsers)
        //             .SingleAsync(comment => comment.NovelCommentId == commentId && comment.IsReply))
        //             .LikeUsers.Add(_context.Users.Single(user => user.UserId == userId));
        //     insertResult = await _context.SaveChangesAsync();
        // }
        // else if (targetType == typeof(ChapterComment))
        // {
        //     updateResult = await _context.ChapterComments!
        //         .Where(comment => comment.ChapterCommentId == commentId && comment.IsReply)
        //         .ExecuteUpdateAsync(
        //             calls => calls.SetProperty(comment => comment.LikeCount, comment => comment.LikeCount + 1)
        //         );
        //     (await _context.ChapterComments!
        //             .Where(comment => comment.ChapterCommentId == commentId && comment.IsReply)
        //             .Include(chapterComment => chapterComment.LikeUsers)
        //             .SingleAsync(comment => comment.ChapterCommentId == commentId && comment.IsReply))
        //             .LikeUsers.Add(_context.Users.Single(user => user.UserId == userId));
        //     insertResult = await _context.SaveChangesAsync();
        // }
        else
        {
            throw new Exception("未知目标类型，无法点赞");
        }

        return updateResult == insertResult ? 1 : 0;
    }
}