using Flyable.Entities;
using Flyable.Filters.Attributes;
using Flyable.StatusCode;
using Flyable.Wraps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using static System.DateTime;

namespace Flyable.Filters;

[AttributeUsage(validOn: AttributeTargets.All,  AllowMultiple = true)]
public class SelfAuthorizeAttribute(UserLevel minLevel = 0, params ushort[]? allowRole) : ActionFilterAttribute
{
    /// <summary>
    ///     0, 站长, 拥有所有权限
    ///     1, 顶级管理员, 拥有管理权限,可以管理编辑与用户
    ///     2-3待定
    ///     4, 编辑, 拥有推荐等权限,可以管理作者
    ///     5-6 待定
    ///     7, 作者
    ///     8, 读者
    /// </summary>
    private readonly ushort[] _role = allowRole ?? UserRole.All;

    private DateTime LastReturnUserTime { get; set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // 标记有AllowAnonymous特性的方法不需要验证
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        User? user;
        try
        {
            user = JsonConvert.DeserializeObject<User>(context.HttpContext.Request.Headers["user"]!);
        }
        catch (Exception e)
        {
            context.Result = new ContentResult
            {
                StatusCode = 200,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(new CodeResult
                {
                    BaseCode = UserStatusCode.NotLogin,
                    Message = "未登录"
                })
            };
            Console.WriteLine(e);
            throw;
        }

        if (user is null)
            context.Result = new ContentResult
            {
                StatusCode = 200,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(new CodeResult
                {
                    BaseCode = UserStatusCode.NotLogin,
                    Message = "未登录"
                })
            };
        else if (!_role.Contains(user.Role))
            context.Result = new ContentResult()
            {
                StatusCode = 200,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(new CodeResult
                {
                    BaseCode = UserStatusCode.RoleNotEnough,
                    Message = "您的权限不足"
                })
            };
        else if (user.Level < minLevel)
            context.Result = new ContentResult
            {
                StatusCode = 200,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(new CodeResult
                {
                    BaseCode = UserStatusCode.LevelNotEnough,
                    Message = "您的等级不足"
                })
            };
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        //如果距离上次返回用户信息时间小于30分钟,则不返回
        if (LastReturnUserTime.AddHours(0.5D) >= Now) return;
        //返回用户token
        context.HttpContext.Response.Headers.Append("user",
            JsonConvert.SerializeObject(context.HttpContext.Items["user"]));
        //记录时间
        LastReturnUserTime = Now;
    }
}