using Flyable.StatusCode;
using Serilog;

namespace Flyable.ThirdParty;

public class WeChatService(IConfiguration configuration, HttpClient httpClient) : IThirdPartyService
{
    private readonly string appId = configuration["WeChat:AppId"] ?? throw new ArgumentNullException("WeChat:AppId");
    private readonly string appSecret = configuration["WeChat:AppSecret"] ?? throw new ArgumentNullException("WeChat:AppSecret");

    /// <summary>
    /// 验证微信账号
    /// </summary>
    /// <param name="code">微信授权码</param>
    /// <returns>微信用户信息</returns>
    public async Task<(int statusCode, string? openId)> VerifyThirdPartyAccountAsync(string code)
    {
        try
        {
            // 通过code获取access_token和openid
            var tokenUrl = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appId}&secret={appSecret}&code={code}&grant_type=authorization_code";
            var response = await httpClient.GetFromJsonAsync<WeChatTokenResponse>(tokenUrl);

            if (response?.OpenId == null)
            {
                return (ThirdPartyStatusCode.ThirdPartyAccountVerifyFailed, null);
            }
            Log.Debug($"""
                "AccessToken": {response.AccessToken},
                "ExpiresIn": {response.ExpiresIn},
                "RefreshToken": {response.RefreshToken},
                "OpenId": {response.OpenId},
                "Scope": {response.Scope}
            """);
            return (ThirdPartyStatusCode.Success, response.OpenId);
        }
        catch (Exception)
        {
            return (ThirdPartyStatusCode.ThirdPartyAccountVerifyFailed, null);
        }
    }
}

/// <summary>
/// 微信Token响应
/// </summary>
public class WeChatTokenResponse
{
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }
    public string? OpenId { get; set; }
    public string? Scope { get; set; }
}