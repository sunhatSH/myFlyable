using Flyable.Repositories.Entities;
using Flyable.StatusCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Flyable.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class SelfAuthorizeAttribute : ActionFilterAttribute
{
    public SelfAuthorizeAttribute(int minLevel = 0, int[]? allowRole = null)
    {
        Role = allowRole ?? new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        MinLevel = minLevel;
    }

    /// <summary>
    ///     0, 站长, 拥有所有权限
    ///     1, 顶级管理员, 拥有管理权限,可以管理编辑与用户
    ///     2-3待定
    ///     4, 编辑, 拥有推荐等权限,可以管理作者
    ///     5-6 待定
    ///     7, 作者
    ///     8, 读者
    /// </summary>
    private int[] Role { get; }

    private int MinLevel { get; }

    private DateTime LastReturnUserTime { get; set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // 标记有AllowAnonymousAttribute的方法不需要验证
        if (context.ActionDescriptor.EndpointMetadata.Any(item => item is AllowAnonymousAttribute)) return;
        User? user = null;
        try
        {
            user = JsonConvert.DeserializeObject<User>(context.HttpContext.Request.Headers["user"]!);
        }
        catch (Exception e) when (e is JsonReaderException or KeyNotFoundException or ArgumentNullException)
        {
            context.Result = new JsonResult(new CodeResult
            {
                BaseCode = UserStatusCode.NotLogin,
                Message = "未登录"
            });
            Console.WriteLine(e);

            return;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (user is null)
            context.Result = (ContentResult)new CodeResult
            {
                BaseCode = UserStatusCode.NotLogin,
                Message = "未登录"
            };
        else if (!Role.Contains(user.Role))
            context.Result = (ContentResult)new CodeResult
            {
                BaseCode = UserStatusCode.UserRoleNotEnough,
                Message = "您的身份不具备该资源访问资格"
            };
        else if (user.Level < MinLevel)
            context.Result = (ContentResult)new CodeResult
            {
                BaseCode = UserStatusCode.UserLevelNotEnough,
                Message = "您的等级不足"
            }; 
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        //如果距离上次返回用户信息时间小于30分钟,则不返回
        if (LastReturnUserTime.AddHours(0.5D) >= DateTime.Now) return;
        //返回用户token
        context.HttpContext.Response.Headers.Add("user",
            JsonConvert.SerializeObject(context.HttpContext.Items["user"]));
        //记录时间
        LastReturnUserTime = DateTime.Now;
    }
}