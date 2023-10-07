using Flyable.Repositories.Entities;
using Flyable.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Flyable.Controllers;

[Route("api/[controller]/[action]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> PublishPost(Post post)
    {
        await _postService.PublishPost(post);
        return new JsonResult(new
        {
            code = 200,
            msg = "发布成功"
        });
    }
}