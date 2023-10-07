using Flyable.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Flyable.Controllers;

[Route("api/[controller]/[action]")]
public class NovelController : ControllerBase
{
    public NovelController(INovelService novelService)
    {
        NovelService = novelService;
    }

    public INovelService NovelService { get; set; }
}