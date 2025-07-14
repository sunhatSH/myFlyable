using Flyable.StatusCode;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
namespace Flyable.ThirdParty;

public class MicrosoftService(IConfiguration configuration, HttpClient httpClient) : IThirdPartyService
{
    public async Task<(int statusCode, string? openId)> VerifyThirdPartyAccountAsync(string code)
    {
        return (ThirdPartyStatusCode.ThirdPartyPlatformNotSupported, null);
    }
}