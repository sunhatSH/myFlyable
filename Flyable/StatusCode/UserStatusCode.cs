namespace Flyable.StatusCode;

public struct UserStatusCode
{
    //-1, 不存在 0, 正常状态 1, 禁止发言 3, 禁止登陆 4, 禁止登陆审核中 5, 已经注销 6, 注销审核中
    public const int Normal                = 1000;
    public const int NotLogin              = 1001;
    public const int NotExist              = 1002;
    public const int NotAdmin              = 1003;
    public const int NotSelf               = 1004;
    public const int LevelNotEnough        = 1005;
    public const int NotFriend             = 1006;
    public const int RoleNotEnough         = 1007;
    public const int Muted                 = 1008;
    public const int MuteChecking          = 1009;
    public const int NotAllowLogin         = 1010;
    public const int NotAllowLoginChecking = 1011;
    public const int Unregistered          = 1012;
}