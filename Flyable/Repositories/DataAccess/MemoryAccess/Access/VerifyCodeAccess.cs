using Flyable.Utils;

namespace Flyable.Repositories.DataAccess.MemoryAccess.Access;

public class VerifyCodeAccess
{
    private const int ExpireTime = 60;
    public string VerifyCode => new VerifyCodeGenerator().VerifyCode;
    private DateTime CreateTime { get; } = DateTime.Now;

    public bool IsExpire => (DateTime.Now - CreateTime).TotalSeconds > ExpireTime;

    public static VerifyCodeAccess GetInstance()
    {
        return new VerifyCodeAccess();
    }
}