using Flyable.StatusCode;
using Flyable.Wraps;
using Masuit.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Flyable.Filters;

/// <summary>
///     检查等级：
///         <br/>
///         详见: <see cref="DetectCode"/>
///         <br/>
///         DetectNothing    = 2000; 不检查
///         <br/>
///         DetectExceptBody = 2001; 只检查标题和其他非正文
///         <br/>
///         DetectAll        = 2002; 检查所有
/// </summary>
public class SensitiveWordsDetectAttribute(int checkLevel) : ActionFilterAttribute
{
    private static List<string> SensitiveWords {
        get
        {
            List<string> words = [];
            words.AddRange(File.ReadAllLines($"{SensitiveWordsFileFolderPath}/SensitiveWords.txt").Distinct()
                .ToList());
            words.AddRange(File.ReadAllLines($"{SensitiveWordsFileFolderPath}/Terror.txt").Distinct()
                .ToList());
            words.AddRange(File.ReadAllLines($"{SensitiveWordsFileFolderPath}/Political.txt").Distinct()
                .ToList());
            words.AddRange(File.ReadAllLines($"{SensitiveWordsFileFolderPath}/Sex.txt").Distinct()
                .ToList());
            return words;
        }
    }
    private static readonly string SensitiveWordsFileFolderPath = Environment.CurrentDirectory + "/SensitiveWords";

    /// <summary>
    ///     key = 检查的标题 例如 title, content
    /// <br/>
    ///     value = 检查的内容 例如 这是我的标题！ 这是我的内容！
    /// <br/>
    ///     key 必须为“title，description，content，addition（作者的话之类的附注）”
    /// </summary>
    private Dictionary<string, string> _checkingContent = [];

    /// <summary>
    ///     被标注的方法执行前，一定含有要检查的数据，因此可以断言“context.ActionArguments.ToDictionary(pair => pair.Key, pair =>
    ///     pair.Value!.ToString())”一定不为空
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context); //在ASP.NET Core中Request Body是Stream的形式
        foreach (var (key, value) in context.ActionArguments) Console.WriteLine($"key = {key}, value = {value}");
        _checkingContent = context.ActionArguments.ToDictionary(pair => pair.Key, pair => pair.Value!.ToString())!;
        switch (checkLevel)
        {
            case DetectCode.DetectExceptBody: // 检查标题和其他非正文
                foreach (var (key, value) in _checkingContent)
                {
                    if (key == "content") continue;
                    if (!SensitiveWords.Any(value.Contains)) continue;
                    context.Result = new ContentResult
                    {
                        StatusCode = 200,
                        ContentType = "application/json",
                        Content = JsonConvert.SerializeObject(new CodeResult
                        {
                            BaseCode = ContentStatusCode.CreateFailed,
                            Message = $"{key switch {
                                "title" => "标题",
                                "description" => "描述",
                                "addition" => "附注",
                                var _ => "未知位置"
                            }}包含敏感词"
                        })
                    };
                    return;
                }
                return;
            case DetectCode.DetectAll: // 检查所有
                foreach (var (key, value) in _checkingContent)
                {
                    Dictionary<string, string>? dict;
                    try
                    {
                        dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    foreach (var line in dict![key].Split("\n"))
                    {
                        if (!SensitiveWords.Any(value.Contains)) continue;
                        context.Result = new ContentResult
                        {
                            StatusCode = 200,
                            ContentType = "application/json",
                            Content = JsonConvert.SerializeObject(new CodeResult
                            {
                                BaseCode = ContentStatusCode.CreateFailed,
                                Message = $"""
                                            第{dict[key].IndexOf(line, StringComparison.Ordinal) + 1}段内容包含敏感词，
                                            绿色分区不允许发表类似内容，该内容如下: {line[..Math.Min(100, line.Length)]}
                                           """
                            })
                        };
                        return;
                    }
                }
                return;
            default: // 不检查
                return;
        }
    }
}