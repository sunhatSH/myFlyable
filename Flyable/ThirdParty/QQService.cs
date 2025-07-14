using System.Net.Http.Json;
using Flyable.StatusCode;
using Serilog;

namespace Flyable.ThirdParty;

public class QQService(IConfiguration configuration, HttpClient httpClient) : IThirdPartyService
{
    private readonly string _appId = configuration["QQ:AppId"] ?? throw new ArgumentNullException("QQ:AppId");
    private readonly string _appKey = configuration["QQ:AppKey"] ?? throw new ArgumentNullException("QQ:AppKey");

    public async Task<(int statusCode, string? openId)> VerifyThirdPartyAccountAsync(string code)
    {
        try
        {
            // 获取 access_token
            var tokenUrl = $"https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id={_appId}&client_secret={_appKey}&code={code}&redirect_uri={Uri.EscapeDataString(configuration["QQ:RedirectUri"] ?? "")}";
            var response = await httpClient.GetStringAsync(tokenUrl);
            var accessToken = response.Split('&').FirstOrDefault(x => x.StartsWith("access_token="))?.Split('=')[1];

            if (string.IsNullOrEmpty(accessToken))
                return (ThirdPartyStatusCode.ThirdPartyAccountVerifyFailed, null);

            // 获取 openid
            var openIdUrl = $"https://graph.qq.com/oauth2.0/me?access_token={accessToken}";
            var openIdResponse = await httpClient.GetStringAsync(openIdUrl);
            var openId = openIdResponse.Split('\"')[3];

            if (string.IsNullOrEmpty(openId))
                return (ThirdPartyStatusCode.ThirdPartyAccountVerifyFailed, null);

            Log.Debug($"""
                "AccessToken": {accessToken},
                "OpenId": {openId}
            """);

            return (ThirdPartyStatusCode.Success, openId);
        }
        catch
        {
            return (ThirdPartyStatusCode.ThirdPartyAccountVerifyFailed, null);
        }
    }
}