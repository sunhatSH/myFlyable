using Flyable.Actions;
using Flyable.Dtos;
using Flyable.Filters;
using Flyable.Filters.Attributes;
using Flyable.StatusCode;
using Flyable.Wraps;
using Flyable.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Flyable.Controllers;

[ApiController]
[Route("api/users")]
[SensitiveWordsDetect(1)]
// [SelfAuthorize(UserLevel.FeatherStart)]
public class UserController(UserAction userAction, VerifyCodeService verifyCodeService) : ControllerBase
{
    /// <summary>
    /// 获取图片验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet("image_verify_code")]
    [AllowAnonymous]
    public async ValueTask<IActionResult> GetImageVerifyCode()
    {
        var (code, imageBytes) = verifyCodeService.GenerateVerifyCode();
        var guid = Guid.NewGuid().ToString();
        await userAction.AddVerifyCodeAsync(guid, code);
        Response.Headers.Append("guid", guid);
        Response.Headers.Append("Access-Control-Expose-Headers", "guid");
        return File(imageBytes, "image/png");
    }

    /// <summary>
    /// 发送邮箱验证码
    /// </summary>
    /// <param name="email"></param>
    /// <param name="username"></param>
    /// <param name="guid"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    [HttpGet("send_verify_code")]
    [AllowAnonymous]
    public async ValueTask<IActionResult> SendVerifyCodeAsync(string email, string username, string guid, [FromQuery] ActionType action)
    {
        Log.Error($"send_verify_code: {email}, {username}, {guid}, {action}");
        var res = await userAction.SendVerifyCodeAsync(email, username, guid, action);
        if (res == SendVerifyCodeStatusCode.Success)
        {
            return Ok();
        }
        return new CodeResult { BaseCode = res };
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="registerDto"></param>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async ValueTask<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto, string guid)
    {
        Log.Error($"register: {registerDto.Username}");
        var res = await userAction.RegisterAsync(registerDto, guid);
        Log.Error($"register: {registerDto.Username} res: {res}");
        if (res != RegisterStatusCode.Success)
        {
            return new CodeResult { BaseCode = res };
        }
        await userAction.RemoveVerifyCodeAsync("verifyCode_" + guid);
        return Ok();
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="user"></param>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async ValueTask<IActionResult> LoginAsync([FromBody] LoginDto user, string guid)
    {
        Log.Error($"login: {user.Username}, GUID: {guid}");

        var (statusCode, userInfo) = await userAction.LoginAsync(user, guid);
        if (statusCode != LoginStatusCode.Success)
        {
            return new CodeResult { BaseCode = statusCode };
        }
        await userAction.RemoveVerifyCodeAsync("verifyCode_" + guid);
        return Ok(new { Success = true, Data = userInfo });
    }

    /// <summary>
    /// 注销用户
    /// </summary>
    /// <param name="user"></param>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpDelete("unregister")]
    public async ValueTask<IActionResult> UnregisterAsync([FromBody] UnregisterDto user, string guid)
    {
        var res = await userAction.UnregisterAsync(user, guid);
        if (res != UnregisterStatusCode.Success)
        {
            return new CodeResult { BaseCode = res };
        }
        await userAction.RemoveVerifyCodeAsync("verifyCode_" + guid);
        await userAction.RemoveVerifyCodeAsync("EmailVerifyCode_" + guid);

        return Ok();
    }

    /// <summary>
    /// 修改用户基础信息
    /// </summary>
    /// <param name="user"></param>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpPost("modify_basic_info")]
    public async ValueTask<IActionResult> ModifyBasicInfoAsync([FromBody] ModifyInfoDto user, string guid)
    {
        await userAction.ModifyMyselfAsync(user);
        return Ok();
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="user"></param>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpPost("modify_password")]
    public async ValueTask<IActionResult> ModifyPasswordAsync([FromBody] ModifyPasswordDto user, string guid)
    {
        await userAction.ModifyPasswordAsync(user);
        return Ok();
    }

    /// <summary>
    /// 修改邮箱
    /// </summary>
    /// <param name="user"></param>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpPost("modify_email")]
    public async ValueTask<IActionResult> ModifyEmailAsync([FromBody] ModifyEmailDto user, string guid)
    {
        await userAction.ModifyEmailAsync(user);
        return Ok();
    }
    /// <summary>
    /// 修改头像
    /// </summary>
    /// <returns></returns>
    [HttpPost("modify_avatar")]
    public async ValueTask<IActionResult> ModifyAvatarAsync([FromBody] ModifyAvatarDto user, string guid)
    {
        await userAction.ModifyAvatarAsync(user);
        return Ok();
    }
    // 绑定第三方账号
    [HttpPost("bind_third_party_account")]
    public async ValueTask<IActionResult> BindThirdPartyAccountAsync([FromBody] BindThirdPartyAccountDto user, string guid)
    {
        await userAction.BindThirdPartyAccountAsync(user);
        return Ok(0);
    }

    // 获取用户收藏的小说
    [HttpGet("{userId}/collections/novels")]
    public async Task<IActionResult> GetUserCollectionNovels(int userId)
    {
        var novels = await userAction.GetUserCollectionNovelsAsync(userId);
        return Ok(new { Success = true, Data = novels });
    }

    // 获取用户收藏的帖子
    [HttpGet("{userId}/collections/posts")]
    public async Task<IActionResult> GetUserCollectionPosts(int userId)
    {
        var posts = await userAction.GetUserCollectionPostsAsync(userId);
        return Ok(new { Success = true, Data = posts });
    }
}