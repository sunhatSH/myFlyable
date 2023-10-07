using Flyable.Filters;
using Flyable.Services.IServices;
using Flyable.Services.ViewModels;
using Flyable.StatusCode;
using Microsoft.AspNetCore.Mvc;

namespace Flyable.Controllers;

[Route("api/[controller]/[action]")]
[SelfAuthorize(allowRole: new[] { 0, 1, 2, 3 })]
public class AdminController : ControllerBase
{
    public AdminController(IAdminService adminService)
    {
        AdminService = adminService;
    }

    private IAdminService AdminService { get; }

    [HttpPost]
    // [SelfAuthorize(allowRole:new[]{0,1,2,3})]
    public async Task<IActionResult> PublishUserTag(UserTagModelView tag)
    {
        var num = await AdminService.PublishUserTag(tag);
        if (num > 0)
            return (ContentResult)new CodeResult
            {
                BaseCode = UserStatusCode.Success,
                Message = "发布成功"
            };
        return (ContentResult)new CodeResult
        {
            BaseCode = UserStatusCode.OperationFailed,
            Message = "因为某种原因，发布标签失败！"
        };
    }
}