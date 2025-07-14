using Flyable.StatusCode;
using Microsoft.AspNetCore.Mvc;

namespace Flyable.Wraps;

public class CodeResult: ActionResult
{
    public int? BaseCode;
    public string? Message;
}