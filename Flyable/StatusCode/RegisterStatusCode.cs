namespace Flyable.StatusCode;

public struct RegisterStatusCode
{
    public const int Success         = 4000;
    public const int UsernameExisted = 4001;
    public const int EmailExisted    = 4002;
#region 图片验证码
    public const int VerifyCodeNotMatch           = 4004;
    public const int VerifyCodeExpiredOrNotExists = 4005;
    public const int VerifyCodeSendFrequently     = 4006;
#endregion

#region 邮箱验证码
    public const int EmailVerifyCodeNotMatch       = 4007;
    public const int EmailVerifyCodeExpired        = 4008;
    public const int EmailVerifyCodeSendFrequently = 4009;
    public const int EmailVerifyCodeSendFailed     = 4010;
#endregion
    public const int UnknownError = 4011;

}