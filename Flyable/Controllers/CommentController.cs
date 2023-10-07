using Flyable.Filters;
using Flyable.Repositories.Entities;
using Flyable.Services.IServices;
using Flyable.Services.ViewModels;
using Flyable.StatusCode;
using Microsoft.AspNetCore.Mvc;

namespace Flyable.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpDelete("{id:int}")]
    public string Test([FromBody] CommentModelView content, [FromRoute] int id)
    {
        Console.WriteLine($"content = {content.Content}");
        Console.WriteLine($"id = {id}");
        return content.Content;
    }


    /// <summary>
    ///     发布评论
    /// </summary>
    /// <param name="content">前端请求的body参数</param>
    /// <returns></returns>
    [HttpPost]
    [SensitiveWordsDetect(SensitiveDetectLevel.DetectAll)]
    public async Task<IActionResult> PublishComment([FromBody] CommentModelView content)
    {
        Console.WriteLine($"content = {content}");
        var type = content.TargetTypeId switch
        {
            ContentType.Post => typeof(Post),
            ContentType.Chapter => typeof(Chapter),
            ContentType.Novel => typeof(Novel),
            ContentType.ChapterComment => typeof(ChapterComment),
            ContentType.PostComment => typeof(PostComment),
            ContentType.NovelComment => typeof(NovelComment),
            var _ => throw new Exception("未知目标类型，无法评论")
        };
        var publishResult = await _commentService.PublishComment(content.Content, type, content.TargetId);
        return publishResult switch
        {
            1 or 2 or 3 or 4 => new CodeResult
            {
                BaseCode = ContentStatusCode.CreateFailed,
                Message = publishResult switch
                {
                    1 => "评论内容为空",
                    2 => "评论内容过长",
                    3 => "评论内容过短",
                    var _ => "评论内容包含敏感词"
                }
            },
            0 => new CodeResult
            {
                BaseCode = ContentStatusCode.Created,
                Message = "评论成功"
            },
            var _ => (ContentResult)new CodeResult
            {
                BaseCode = ContentStatusCode.CreateFailed,
                Message = "未知错误"
            }
        };
    }


    [HttpGet]
    public async Task<IActionResult> DeleteComment(int commentId, int targetTypeId)
    {
        var type = targetTypeId switch
        {
            0 => typeof(Post),
            1 => typeof(Chapter),
            2 => typeof(Novel),
            3 => typeof(ChapterComment),
            4 => typeof(PostComment),
            5 => typeof(NovelComment),
            var _ => throw new Exception("未知目标类型，删除失败")
        };
        var result = await _commentService.DeleteComment(commentId, type);
        return result switch
        {
            -1 => new CodeResult
            {
                BaseCode = ContentStatusCode.AlterFailed,
                Message = "评论删除失败，权限不足"
            },
            0 => new CodeResult
            {
                BaseCode = ContentStatusCode.Altered,
                Message = "评论删除成功"
            },
            1 => new CodeResult
            {
                BaseCode = ContentStatusCode.AlterFailed,
                Message = "评论删除失败，请重试"
            },
            var _ => (ContentResult)new CodeResult
            {
                BaseCode = ContentStatusCode.AlterFailed,
                Message = "未知错误"
            }
        };
    }

    [HttpPost("{id:int}")]
    public async Task<IActionResult> EditComment([FromBody] CommentModelView content, [FromRoute] int id)
    {
        var type = content.TargetTypeId switch
        {
            ContentType.Post => typeof(Post),
            ContentType.Chapter => typeof(Chapter),
            ContentType.Novel => typeof(Novel),
            ContentType.ChapterComment => typeof(ChapterComment),
            ContentType.PostComment => typeof(PostComment),
            ContentType.NovelComment => typeof(NovelComment),
            var _ => throw new Exception("未知目标类型，无法更新")
        };
        var publishResult = await _commentService.UpdateComment(id, type, content.Content);
        return publishResult switch
        {
            -1 => new CodeResult
            {
                BaseCode = ContentStatusCode.AlterFailed,
                Message = "评论更新失败，权限不足"
            },
            1 or 2 or 3 or 4 => new CodeResult
            {
                BaseCode = ContentStatusCode.CreateFailed,
                Message = publishResult switch
                {
                    1 => "评论内容为空",
                    2 => "评论内容过长",
                    3 => "评论内容过短",
                    var _ => "评论内容包含敏感词"
                }
            },
            0 => new CodeResult
            {
                BaseCode = ContentStatusCode.Created,
                Message = "评论成功"
            },
            var _ => (ContentResult)new CodeResult
            {
                BaseCode = ContentStatusCode.CreateFailed,
                Message = "未知错误"
            }
        };
    }
}