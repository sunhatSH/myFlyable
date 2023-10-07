using Masuit.Tools.Security;
using Microsoft.Extensions.Configuration.Json;

namespace Flyable.Extensions;

public static class SecretTokenFactory
{
    static SecretTokenFactory()
    {
        const string path = "./appsettings.Development.json";
        var config = new ConfigurationBuilder().Add(new JsonConfigurationSource
        {
            Path = path, ReloadOnChange = true
        }).Build();
        Key = config["Key"]!;
    }

    private static string Key { get; }

    /// <summary>
    ///     Token 使用AES加密username,秘钥使用
    /// </summary>
    /// <param name="userJson">用户信息</param>
    /// <returns></returns>
    public static string GetUserToken(this string userJson)
    {
        return userJson.AESEncrypt(Key);
    }


    /// <summary>
    ///     使用AES加密password， 秘钥使用Key
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string GetPasswordToken(this string password)
    {
        return password.AESEncrypt(Key);
    }
}