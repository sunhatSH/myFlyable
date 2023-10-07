using Flyable.StatusCode;
using Masuit.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Flyable.Filters;

/// <summary>
///     需要对内容检测的方法标注该特性
/// </summary>
public class SensitiveWordsDetectAttribute : ActionFilterAttribute
{
    private static readonly List<string> SensitiveWords = new();
    private static readonly string SensitiveWordsFileFolderPath = Environment.CurrentDirectory + "/SensitiveWords";

    private readonly int _checkLevel;

    /// <summary>
    ///     key = 检查的标题 例如 title, content
    ///     value = 检查的内容 例如 这是我的标题！ 这是我的内容！
    ///     key 必须为“title，description，content，addition（作者的话之类的附注）”
    /// </summary>
    private Dictionary<string, string> _checkingContent = new();

    static SensitiveWordsDetectAttribute()
    {
        using var streamOfAdOrPrivate = File.Open($"{SensitiveWordsFileFolderPath}/AdvertisementOrPrivateAccount.txt",
            FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        var reader = new StreamReader(streamOfAdOrPrivate);
        var adOrPrivacy = reader.ReadAllLines().Distinct();
        SensitiveWords.AddRange(adOrPrivacy);

        using var streamOfPolitical = File.Open($"{SensitiveWordsFileFolderPath}/Political.txt",
            FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        reader = new StreamReader(streamOfPolitical);
        var political = reader.ReadAllLines().Distinct();
        SensitiveWords.AddRange(political);

        using var streamOfTerror = File.Open($"{SensitiveWordsFileFolderPath}/Terror.txt",
            FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        reader = new StreamReader(streamOfTerror);
        var terror = reader.ReadAllLines().Distinct();
        SensitiveWords.AddRange(terror);

        using var streamOfSex = File.Open($"{SensitiveWordsFileFolderPath}/Sex.txt",
            FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        reader = new StreamReader(streamOfSex);
        var sex = reader.ReadAllLines().Distinct();
        SensitiveWords.AddRange(sex);
    }

    /// <summary>
    ///     检查等级：
    ///     0:对任何内容都不检查
    ///     1:对标题和其他非正文检查
    ///     2:对所有内容，包括正文都进行检查
    /// </summary>
    public SensitiveWordsDetectAttribute(int checkLevel)
    {
        _checkLevel = checkLevel;
    }

    /// <summary>
    ///     被标注的方法执行前，一定含有要检查的数据，因此可以断言“context.ActionArguments.ToDictionary(pair => pair.Key, pair =>
    ///     pair.Value!.ToString())”一定不为空
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        //在ASP.NET Core中Request Body是Stream的形式
        var stream = new StreamReader(context.HttpContext.Request.Body);
        foreach (var (key, value) in context.ActionArguments) Console.WriteLine($"key = {key}, value = {value}");
        _checkingContent = context.ActionArguments.ToDictionary(pair => pair.Key, pair => pair.Value!.ToString())!;
        switch (_checkLevel)
        {
            case SensitiveDetectLevel.DetectExceptBody: // 检查标题和其他非正文
                foreach (var (key, value) in _checkingContent)
                {
                    if (key.ToLower().Contains("content")) continue;
                    if (!SensitiveWords.Any(value.Contains)) continue;
                    context.Result = new JsonResult(new CodeResult
                    {
                        BaseCode = ContentStatusCode.CreateFailed,
                        Message = "非正文内容包含敏感词"
                    });
                    break;
                }

                return;
            case SensitiveDetectLevel.DetectAll: // 检查所有
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

                    foreach (var line in dict![key].Split(" "))
                    {
                        if (!SensitiveWords.Any(value.Contains)) continue;
                        context.Result = new JsonResult(new CodeResult
                        {
                            BaseCode = ContentStatusCode.CreateFailed,
                            Message = $"第{dict![key].IndexOf(line, StringComparison.Ordinal) + 1}段内容包含敏感词，" +
                                      $"绿色分区不允许发表类似内容，该内容如下: {line[..Math.Min(100, line.Length)]}"
                        });
                        break;
                    }
                }

                return;
            default: // 不检查
                return;
        }
    }
}