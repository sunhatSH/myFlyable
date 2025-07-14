namespace Flyable.StatusCode;

public struct LoginStatusCode
{
    public const int Success      = 5000;
    public const int NotExist     = 5001;
    public const int PwdIncorrect = 5002;
    public const int VerifyError  = 5003;
    public const int NotActive    = 5004;
    public const int Unknown      = 5005;
}