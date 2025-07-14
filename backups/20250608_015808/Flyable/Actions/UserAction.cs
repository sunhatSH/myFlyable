using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Flyable.Dtos;
using Flyable.Entities;
using Flyable.StatusCode;
using Flyable.Utils;
using Flyable.Wraps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Flyable.ThirdParty;

namespace Flyable.Actions;

public class UserAction(
    FlyableUserContext context,
    IDistributedCache cache,
    ThirdPartyAccountService thirdPartyAccountService)
{
    // 图片验证码
    public async ValueTask AddVerifyCodeAsync(string guid, string? code)
    {
        var verifyCode = code ?? VerifyCodeGenerator.VerifyCode.ToString();
        await cache.SetStringAsync("verifyCode_" + guid, verifyCode, new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = DateTime.Now.AddSeconds(60)
        });
    }

    // 邮箱验证码
    public async ValueTask<int> SendVerifyCodeAsync(string email, string username, string guid, ActionType action)
    {
        var code = VerifyCodeGenerator.VerifyCode.ToString();

        try
        {
            var smtpClient = new SmtpClient("smtp.qq.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(SettingsReader.GetSetting("SmtpUsername"), SettingsReader.GetSetting("QQEmailAuthCode"));
            var message = new MailMessage();
            message.From = new MailAddress(SettingsReader.GetSetting("SmtpUsername"));
            message.To.Add(email);
            message.Subject = "Flyable 验证码";
            message.Body = new TemplateTranslate(action, username, code).Template;
            message.IsBodyHtml = true;
            await smtpClient.SendMailAsync(message);
        }
        catch (Exception e)
        {
            // TODO: 添加日志记录: logger.LogError(e, "操作失败");
            return SendVerifyCodeStatusCode.Failed;
        }
        await cache.SetStringAsync("EmailVerifyCode_" + guid, code, new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = DateTime.Now.AddSeconds(300)
        });
        return SendVerifyCodeStatusCode.Success;
    }
    public async ValueTask<string?> GetVerifyCodeAsync(string guid)
    {
        return await cache.GetStringAsync("verifyCode_" + guid);
    }

    public async ValueTask<int> RegisterAsync(RegisterDto user, string guid)
    {
        if (await context.Users.AnyAsync(userInDb => userInDb.Username == user.Username))
        {
            return RegisterStatusCode.UsernameExisted;
        }

        if (await context.Users.AnyAsync(userInDb => userInDb.Email == user.Email))
        {
            return RegisterStatusCode.EmailExisted;
        }

        var value = await GetVerifyCodeAsync(guid);
        if (value == null)
        {
            return RegisterStatusCode.VerifyCodeExpiredOrNotExists;
        }

        if (value != user.VerifyCode)
        {
            return RegisterStatusCode.VerifyCodeNotMatch;
        }

        await context.Users.AddAsync(new User
        {
            Username = user.Username,
            PwdToken = user.PwdToken,
            Email = user.Email,
            Role = UserRole.Reader,
            RegisterTime = DateTime.Now,
            LastLoginTime = DateTime.Now,
            Ip = user.Ip,
            Status = UserStatusCode.Normal,
            Level = UserLevel.FeatherStart,
            TagList = [],
            WearTag = new UserTag
            {
                TagName = "默认标签",
                TagIcon = "https://cdn.jsdelivr.net/gh/Flyable-Team/Flyable-Resource/Flyable/Tag/default.png",
                TagDescription = "默认标签",
                LastPublishTime = DateTime.Now,
                CreatedTime = DateTime.Now,
                TagRemain = 0
            },
            CollectionNovels = [],
            CollectionPosts = [],
            Follows = [],
            Followed = [],
            ColorStone = Currency.InitialColorStone,
            Feather = Currency.InitialFeather,
            ReportCount = 0,
            LastModifyTime = DateTime.Now,
            LastModifyAdmin = null,
            SelfIntroduction = "",
            IsVip = false,
            HeadLink = "https://cdn.jsdelivr.net/gh/Flyable-Team/Flyable-Resource/Flyable/Head/default.png"
        });
        await context.SaveChangesAsync();
        return RegisterStatusCode.Success;
    }

    public async ValueTask<int> LoginAsync(LoginDto user, string guid)
    {
        var verifyCode = await GetVerifyCodeAsync(guid);
        if (verifyCode != user.VerifyCode)
        {
            return LoginStatusCode.VerifyError;
        }
        var userInDb = await context.Users.SingleOrDefaultAsync(user1 => user1.Username == user.Username);
        if (userInDb is null)
        {
            return LoginStatusCode.NotExist;
        }

        if (userInDb.PwdToken != user.PwdToken)
        {
            return LoginStatusCode.PwdIncorrect;
        }
        userInDb.LastLoginTime = DateTime.Now;
        userInDb.Ip = user.Ip;
        await context.SaveChangesAsync();
        return userInDb.Status is UserStatusCode.Unregistered or UserStatusCode.NotAllowLogin ?
            LoginStatusCode.NotActive : LoginStatusCode.Success;
    }

    public async ValueTask<int> UnregisterAsync(UnregisterDto user, string guid)
    {
        if (user.VerifyCode != await GetVerifyCodeAsync(guid))
        {
            return UnregisterStatusCode.VerifyCodeNotMatch;
        }

        if (user.EmailVerifyCode != await cache.GetStringAsync("EmailVerifyCode_" + guid))
        {
            return UnregisterStatusCode.EmailVerifyNotMatch;
        }

        var userInDb = await context.Users.SingleOrDefaultAsync(user1 => user1.Username == user.Username);
        if (userInDb is null || userInDb.Status == UserStatusCode.Unregistered)
        {
            return UnregisterStatusCode.NotFound;
        }

        if (userInDb.Status == UserStatusCode.NotAllowLogin)
        {
            return UnregisterStatusCode.Locked;
        }

        if (userInDb.PwdToken != user.PwdToken)
        {
            return UnregisterStatusCode.PwdNotMatch;
        }
        userInDb.Status = UserStatusCode.Unregistered;
        var res = await context.SaveChangesAsync();
        return res > 0 ? UnregisterStatusCode.Success : UnregisterStatusCode.UnknownError;
    }

    public async Task RemoveVerifyCodeAsync(string key)
    {
        await cache.RemoveAsync(key);
    }

    public async Task<int> ModifyMyselfAsync(ModifyInfoDto user)
    {
        var userInDb = await context.Users.SingleOrDefaultAsync(user1 => user1.Username == user.OldUsername);
        switch (userInDb)
        {
            case null:
                return ModifyStatusCode.UserNotExist;
            case { Status: UserStatusCode.NotAllowLogin or UserStatusCode.NotAllowLoginChecking }:
                return ModifyStatusCode.StatusError;
        }

        if (user.NewUsername is not null && user.NewUsername != user.OldUsername)
        {
            if (await context.Users.AnyAsync(user1 => user1.Username == user.NewUsername))
            {
                return ModifyStatusCode.UserNewNameExist;
            }
            return 9;
        }
        userInDb.SelfIntroduction = user.SelfIntroduction;
        userInDb.Username = user.NewUsername;
        userInDb.WearTag = user.WearTag;
        var res = await context.SaveChangesAsync();
        return res == 1 ? ModifyStatusCode.Success : ModifyStatusCode.UnknownError;
    }

    public async Task<int> ModifyPasswordAsync(ModifyPasswordDto user)
    {
        var userInDb = await context.Users.SingleOrDefaultAsync(user1 => user1.Username == user.Username);
        if (userInDb is null)
        {
            return ModifyStatusCode.UserNotExist;
        }

        if (userInDb.Status is UserStatusCode.NotAllowLogin or UserStatusCode.NotAllowLoginChecking)
        {
            return ModifyStatusCode.StatusError;
        }

        if (userInDb.PwdToken != user.OldPwdToken)
        {
            return ModifyStatusCode.UserPasswordError;
        }

        userInDb.PwdToken = user.NewPwdToken;
        var res = await context.SaveChangesAsync();
        return res == 1 ? ModifyStatusCode.Success : ModifyStatusCode.UnknownError;
    }

    public async Task<int> ModifyEmailAsync(ModifyEmailDto user)
    {
        var userInDb = await context.Users.SingleOrDefaultAsync(user1 => user1.Username == user.Username);
        if (userInDb is null)
        {
            return ModifyStatusCode.UserNotExist;
        }

        if (userInDb.Status is UserStatusCode.NotAllowLogin or UserStatusCode.NotAllowLoginChecking)
        {
            return ModifyStatusCode.StatusError;
        }

        if (userInDb.PwdToken != user.PwdToken)
        {
            return ModifyStatusCode.UserPasswordError;
        }

        userInDb.Email = user.NewEmail;
        var res = await context.SaveChangesAsync();
        return res == 1 ? ModifyStatusCode.Success : ModifyStatusCode.UnknownError;
    }

    public async Task<int> ModifyAvatarAsync(ModifyAvatarDto user)
    {
        var userInDb = await context.Users.SingleOrDefaultAsync(user1 => user1.Username == user.Username);
        if (userInDb is null)
        {
            return ModifyStatusCode.UserNotExist;
        }

        if (userInDb.Status is UserStatusCode.NotAllowLogin or UserStatusCode.NotAllowLoginChecking)
        {
            return ModifyStatusCode.StatusError;
        }

        userInDb.HeadLink = user.AvatarUrl;
        var res = await context.SaveChangesAsync();
        return res == 1 ? ModifyStatusCode.Success : ModifyStatusCode.UnknownError;
    }

    public async Task<int> BindThirdPartyAccountAsync(BindThirdPartyAccountDto user)
    {
        var userInDb = await context.Users.SingleOrDefaultAsync(user1 => user1.Username == user.Username);
        if (userInDb is null)
        {
            return ModifyStatusCode.UserNotExist;
        }

        if (userInDb.Status is UserStatusCode.NotAllowLogin or UserStatusCode.NotAllowLoginChecking)
        {
            return ModifyStatusCode.StatusError;
        }
        // 判断第三方账号是否已经存在
        if (userInDb.ThirdPartyAccount?.ContainsKey(Enum.Parse<ThirdPartyType>(user.ThirdPartyAccount.Split('_')[0])) ?? false)
        {
            return ThirdPartyStatusCode.ThirdPartyAccountAlreadyExists;
        }

        var thirdPartyService = thirdPartyAccountService.GetThirdPartyServiceAsync(user.ThirdPartyAccount);
        // 对接第三方平台，如果成功，解析第三方账号并添加到数据库
        var res = await thirdPartyAccountService.BindThirdPartyAccountAsync(userInDb, user.ThirdPartyAccount, thirdPartyService);
        if (res != ThirdPartyStatusCode.Success)
        {
            return res;
        }
        res = await context.SaveChangesAsync();
        return res == 1 ? ModifyStatusCode.Success : ModifyStatusCode.UnknownError;
    }
}