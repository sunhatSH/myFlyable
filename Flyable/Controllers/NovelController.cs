using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Flyable.Actions;
using Flyable.Dtos;
using Flyable.StatusCode;
using Flyable.Wraps;

namespace Flyable.Controllers;

[ApiController]
[Route("api/novel")]
public class NovelController(NovelAction novelAction) : ControllerBase
{
    /// <summary>
    /// 获取小说列表
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetNovelList([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new NovelQueryDto { Page = page, PageSize = pageSize };
            var result = await novelAction.GetNovelListAsync(query);
            return Ok(new ApiResponse<object> { Data = result, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 根据ID获取小说详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNovel(int id)
    {
        try
        {
            var result = await novelAction.GetNovelByIdAsync(id);
            if (result == null)
                return NotFound(new ApiResponse<object> { Message = "小说不存在", Success = false });

            return Ok(new ApiResponse<object> { Data = result, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 创建新小说
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateNovel([FromBody] CreateNovelDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // 如果没有设置AuthorId，使用默认值1
            if (dto.AuthorId == 0)
            {
                dto.AuthorId = 1;
            }

            var result = await novelAction.CreateNovelAsync(dto);
            return Ok(new ApiResponse<object> { Data = result, Success = true, Message = "小说创建成功" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 修改小说信息
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNovel(int id, [FromBody] UpdateNovelDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await novelAction.UpdateNovelAsync(id, dto);
            return Ok(new ApiResponse<object> { Data = result, Success = true, Message = "小说信息更新成功" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 删除小说
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNovel(int id)
    {
        try
        {
            var result = await novelAction.DeleteNovelAsync(id);
            if (!result)
                return NotFound(new ApiResponse<object> { Message = "小说不存在", Success = false });

            return Ok(new ApiResponse<object> { Success = true, Message = "小说删除成功" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 为小说添加章节
    /// </summary>
    [HttpPost("{novelId}/chapters")]
    public async Task<IActionResult> AddChapter(int novelId, [FromBody] CreateChapterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await novelAction.AddChapterAsync(novelId, dto);
            return Ok(new ApiResponse<object> { Data = result, Success = true, Message = "章节添加成功" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 获取小说章节列表
    /// </summary>
    [HttpGet("{novelId}/chapters")]
    public async Task<IActionResult> GetChapters(int novelId)
    {
        try
        {
            var result = await novelAction.GetChaptersByNovelIdAsync(novelId);
            return Ok(new ApiResponse<object> { Data = result, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 获取章节内容
    /// </summary>
    [HttpGet("{novelId}/chapters/{chapterId}")]
    public async Task<IActionResult> GetChapter(int novelId, int chapterId)
    {
        try
        {
            var result = await novelAction.GetChapterAsync(novelId, chapterId);
            if (result == null)
                return NotFound(new ApiResponse<object> { Message = "章节不存在", Success = false });

            return Ok(new ApiResponse<object> { Data = result, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 为小说添加评论
    /// </summary>
    [HttpPost("{novelId}/comments")]
    public async Task<IActionResult> AddComment(int novelId, [FromBody] CreateCommentDto dto)
    {
        try
        {
            var comment = await novelAction.AddCommentAsync(novelId, dto);
            // 只返回简单DTO，避免循环引用
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = new
                {
                    comment.NovelCommentId,
                    comment.Content,
                    comment.UserId,
                    comment.PublishTime
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 获取小说评论列表
    /// </summary>
    [HttpGet("{novelId}/comments")]
    public async Task<IActionResult> GetComments(int novelId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var result = await novelAction.GetCommentsByNovelIdAsync(novelId, page, pageSize);
            return Ok(new ApiResponse<object> { Data = result, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 搜索小说
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> SearchNovels([FromQuery] string keyword, [FromQuery] string category = null)
    {
        try
        {
            var result = await novelAction.SearchNovelsAsync(keyword, category);
            return Ok(new ApiResponse<object> { Data = result, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 获取小说榜单
    /// </summary>
    [HttpGet("rankings/{type}")]
    public async Task<IActionResult> GetNovelRankings(string type, [FromQuery] int limit = 20)
    {
        try
        {
            var result = await novelAction.GetNovelRankingsAsync(type, limit);
            return Ok(new ApiResponse<object> { Data = result, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 获取推荐小说
    /// </summary>
    [HttpGet("recommendations")]
    public async Task<IActionResult> GetRecommendations([FromQuery] int limit = 10)
    {
        try
        {
            var result = await novelAction.GetRecommendationsAsync(limit);
            return Ok(new ApiResponse<object> { Data = result, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 获取小说详情（包含章节列表），支持 userId 查询点赞状态
    /// </summary>
    [HttpGet("{id}/detail")]
    public async Task<IActionResult> GetNovelDetail(int id, [FromQuery] int? userId = null)
    {
        try
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            await novelAction.RecordNovelViewAsync(id, userId, ip);
            var result = await novelAction.GetNovelDetailAsync(id, userId);
            if (result == null)
                return NotFound(new ApiResponse<object> { Message = "小说不存在", Success = false });

            return Ok(new ApiResponse<object> { Data = result, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 为小说评分
    /// </summary>
    [HttpPost("{id}/rate")]
    public async Task<IActionResult> RateNovel(int id, [FromBody] RateNovelDto dto)
    {
        try
        {
            var result = await novelAction.RateNovelAsync(id, dto);
            return Ok(new ApiResponse<object> { Data = result, Success = true, Message = "评分成功" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 收藏/取消收藏小说
    /// </summary>
    [HttpPost("{id}/collect")]
    public async Task<IActionResult> ToggleCollectNovel(int id, [FromQuery] int userId)
    {
        try
        {
            var result = await novelAction.ToggleCollectNovelAsync(id, userId);
            return Ok(new ApiResponse<object> { Data = result, Success = true, Message = result ? "收藏成功" : "已取消收藏" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 修改小说文案
    /// </summary>
    [HttpPut("{id}/copywriting")]
    public async Task<IActionResult> UpdateCopywriting(int id, [FromBody] CopywritingDto dto)
    {
        var novel = await novelAction.GetNovelByIdAsync(id);
        if (novel == null) return NotFound(new ApiResponse<object> { Message = "小说不存在", Success = false });
        novel.Copywriting = dto.Copywriting;
        novel.SellingPoint = dto.SellingPoint;
        novel.Recommendation = dto.Recommendation;
        await novelAction.SaveNovelAsync(novel);
        return Ok(new ApiResponse<object> { Success = true });
    }

    /// <summary>
    /// 获取小说文案
    /// </summary>
    [HttpGet("{id}/copywriting")]
    public async Task<IActionResult> GetCopywriting(int id)
    {
        var novel = await novelAction.GetNovelByIdAsync(id);
        if (novel == null) return NotFound(new ApiResponse<object> { Message = "小说不存在", Success = false });
        return Ok(new ApiResponse<object>
        {
            Data = new
            {
                novel.Copywriting,
                novel.SellingPoint,
                novel.Recommendation
            },
            Success = true
        });
    }

    /// <summary>
    /// 获取每本小说的最新一章，按更新时间倒序
    /// </summary>
    [HttpGet("latest-updates")]
    public async Task<IActionResult> GetLatestNovelUpdates([FromQuery] int limit = 10)
    {
        var result = await novelAction.GetLatestNovelUpdatesAsync(limit);
        return Ok(new ApiResponse<object> { Data = result, Success = true });
    }

    /// <summary>
    /// 从txt文件解析并导入小说和章节，如果存在同名小说则阻止上传，作者使用当前用户sunhat
    /// </summary>
    [HttpPost("import-txt")]
    public async Task<IActionResult> ImportNovelFromTxt([FromBody] string txtContent)
    {
        var result = await novelAction.ImportNovelFromTxtAsync(txtContent);
        if (result)
        {
            return Ok(new ApiResponse<object> { Success = true, Message = "导入成功" });
        }
        else
        {
            return BadRequest(new ApiResponse<object> { Success = false, Message = "同名小说已存在，导入失败" });
        }
    }

    /// <summary>
    /// 为小说点赞/取消点赞
    /// </summary>
    [HttpPost("{novelId}/like")]
    public async Task<IActionResult> ToggleLikeNovel(int novelId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var liked = await novelAction.ToggleLikeNovelAsync(novelId, userId);
            return Ok(new ApiResponse<object> { Data = liked, Success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object> { Message = ex.Message, Success = false });
        }
    }

    /// <summary>
    /// 获取小说点赞数
    /// </summary>
    [HttpGet("{novelId}/like/count")]
    public async Task<IActionResult> GetNovelLikeCount(int novelId)
    {
        var count = await novelAction.GetNovelLikeCountAsync(novelId);
        return Ok(new { count });
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out var userId))
            return userId;
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer temp_token_"))
        {
            return 1;
        }
        return 1;
    }
}