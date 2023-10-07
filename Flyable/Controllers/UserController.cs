using Flyable.Filters;
using Flyable.Services.IServices;
using Flyable.Services.ViewModels;
using Flyable.StatusCode;
using Masuit.Tools.Strings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Encoders;

namespace Flyable.Controllers;

/// <summary>
///     默认允许所有角色访问
/// </summary>
[Route("api/[controller]/[action]")]
[EnableCors]
[SelfAuthorize]
public class UserController : ControllerBase
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<UserController> _logger;
    private readonly IUserBaseService _userBaseService;

    public UserController(IUserBaseService userBaseService, IDistributedCache cache, ILogger<UserController> logger)
    {
        _userBaseService = userBaseService;
        _cache = cache;
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(UserModelView userModel, string password, string verifyCode)
    {
        var result = await _userBaseService.Register(userModel, password, verifyCode);
        if (result == null)
            return (ContentResult)new CodeResult
            {
                BaseCode = UserStatusCode.Failed,
                Message = "注册失败"
            };

        return (ContentResult)new CodeResult
        {
            BaseCode = UserStatusCode.Created,
            Message = "注册成功"
        };
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password,
        [FromForm] string verifyCode)
    {
        var result = await _userBaseService.Login(username, password, verifyCode);
        if (result == UserStatusCode.LoginFailed)
            return (ContentResult)new CodeResult
            {
                BaseCode = UserStatusCode.LoginFailed,
                Message = "登录失败"
            };

        return (ContentResult)new CodeResult
        {
            BaseCode = UserStatusCode.Success,
            Message = JsonConvert.SerializeObject(_userBaseService.GetUserModelView(username))
        };
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetVerifyCode()
    {
        var verifyCode = _userBaseService.GetVerifyCode();
        var stream = verifyCode.CreateValidateGraphic();
        // 将stream以图片形式返回给浏览器
        return new JsonResult(new
        {
            image = Base64.ToBase64String(stream.ToArray()).ReplaceLineEndings(""),
            guid = Guid.NewGuid().ToString()
        });
        // return File(stream, "image/png");
    }

    [HttpPost]
    public async Task<IActionResult> DestroyAccount([FromForm] string username, [FromForm] string password,
        [FromForm] string verifyCode)
    {
        var result = await _userBaseService.DestroyAccount(username, password, verifyCode);
        if (result == UserStatusCode.DestroyFailed)
            return (ContentResult)new CodeResult
            {
                BaseCode = UserStatusCode.DestroyFailed,
                Message = "注销失败"
            };

        return (ContentResult)new CodeResult
        {
            BaseCode = UserStatusCode.Destroyed,
            Message = "注销成功"
        };
    }

    /**
     * <summary>
     *     修改用户信息, 只能全部修改或者全不修改
     * </summary>
     * <param name="alteredItem"></param>
     * <returns></returns>
     * @api {post} /api/User/UpdateUser 修改用户信息
     */
    [HttpPost]
    public async Task<IActionResult> UpdateUser(params dynamic[] alteredItem)
    {
        var (status, newUser) = await _userBaseService.UpdateUser(alteredItem);
        if (status != UserStatusCode.UpdateUserSuccess)
            return (ContentResult)new CodeResult
            {
                BaseCode = status,
                Message = "修改用户信息失败"
            };
        return (ContentResult)new CodeResult
        {
            BaseCode = UserStatusCode.Accepted,
            Message = JsonConvert.SerializeObject(newUser)
        };
    }

    [HttpGet]
    public async Task<IActionResult> FindAllUsers()
    {
        return (ContentResult)new CodeResult
        {
            BaseCode = UserStatusCode.Accepted,
            Message = JsonConvert.SerializeObject(await _userBaseService.FindAllUsers())
        };
    }

    [HttpPost]
    public async Task<IActionResult> FollowUser([FromForm] string username)
    {
        var result = await _userBaseService.FollowUser(username);
        if (result == UserStatusCode.OperationFailed)
            return (ContentResult)new CodeResult
            {
                BaseCode = UserStatusCode.OperationFailed,
                Message = "操作失败"
            };

        return (ContentResult)new CodeResult
        {
            BaseCode = UserStatusCode.Accepted,
            Message = "操作成功"
        };
    }

    [HttpPost]
    public async Task<IActionResult> UnFollowUser([FromForm] int userId)
    {
        var result = await _userBaseService.UnFollowUser(userId);
        if (result == UserStatusCode.OperationFailed)
            return (ContentResult)new CodeResult
            {
                BaseCode = UserStatusCode.OperationFailed,
                Message = "操作失败"
            };
        return (ContentResult)new CodeResult
        {
            BaseCode = UserStatusCode.Accepted,
            Message = "操作成功"
        };
    }
}