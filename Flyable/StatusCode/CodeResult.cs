using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Flyable.StatusCode;

public struct CodeResult
{
    public int BaseCode { get; set; }
    public string? Message { get; set; }

    public static implicit operator ContentResult(CodeResult codeResult)
    {
        return new ContentResult
        {
            Content = JsonConvert.SerializeObject(codeResult)
        };
    }
}