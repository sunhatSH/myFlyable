namespace Flyable.StatusCode;

public struct ModifyStatusCode
{
    public const int Success = 7000;
    public const int UserNotExist = 7001;
    public const int UserNewNameExist = 7002;
    public const int UserNewEmailExist = 7003;
    public const int UserEmailVerifyErrorOrExpired = 7004;
    public const int StatusError = 7005;
    public const int UserPasswordError = 7006;
    public const int VerifyCodeErrorOrExpired = 7007;
    public const int UserStatusError = 7008;
    public const int UnknownError = 7009;
}