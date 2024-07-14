using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Flyable.StatusCode;

public struct CodeResult
{
    public int BaseCode { get; set; }
    public string? Message { get; set; }

    public delegate string Delegate1(int a);
    public static implicit operator ContentResult(CodeResult codeResult)
    {
        return new ContentResult
        {
            Content = JsonConvert.SerializeObject(codeResult)
        };
    }
}