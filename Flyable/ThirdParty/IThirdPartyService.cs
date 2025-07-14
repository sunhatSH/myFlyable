using Flyable.Entities;

public interface IThirdPartyService
{
    abstract Task<(int statusCode, string? openId)> VerifyThirdPartyAccountAsync(string code);
}