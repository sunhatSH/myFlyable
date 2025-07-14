namespace Flyable.StatusCode;

public struct UnregisterStatusCode
{
    public const int Success              = 6000;
    public const int Locked               = 6001;
    public const int NotFound             = 6002;
    public const int NotAllowed           = 6003;
    public const int AuthorizeError       = 6004;
    public const int NotAcceptable        = 6005;
    public const int ServerError          = 6006;
    public const int UnknownError         = 6007;
    public const int VerifyCodeNotMatch   = 6008;
    public const int PwdNotMatch          = 6009;
    public const int EmailVerifyNotMatch  = 6010;
    public const int SendEmailFailed      =   6011;
}