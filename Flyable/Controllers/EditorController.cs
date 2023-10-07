using Flyable.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Flyable.Controllers;

[Route("api/[controller]/[action]")]
public class EditorController
{
    public EditorController(EditorService editorService)
    {
        EditorService = editorService;
    }

    public EditorService EditorService { get; set; }
}