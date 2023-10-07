namespace Flyable.StatusCode;

public struct UserStatusCode
{
    /// 注册成功
    public const int Created = 1201;


    public const int Accepted = 1202;

    /// 注册失败
    public const int Failed = 1203;

    /// 用户名已存在
    public const int UsernameExists = 1204;

    /// 登录失败
    public const int LoginFailed = 1205;

    ///用户已被禁
    public const int Banned = 1206;

    ///用户被禁止登录
    public const int ForbiddenToLogin = 1207;

    ///用户被禁言
    public const int ForbiddenToSpeak = 1208;

    ///用户账号已被删除
    public const int UserDeleted = 1209;

    ///用户角色不满足访问资格
    public const int UserRoleNotEnough = 1210;

    ///用户等级不足
    public const int UserLevelNotEnough = 1211;

    /// 其他原因无法访问
    public const int OtherReason = 1212;

    /// 登陆成功
    public const int Success = 1213;

    // 未登录
    public const int NotLogin = 1214;


    /// 操作因某种原因失败
    public const int OperationFailed = 1215;

    /// 注销成功
    public const int Destroyed = 1216;

    /// 注销失败
    public const int DestroyFailed = 1217;

    /// 修改用户信息成功
    public const int UpdateUserSuccess = 1218;

    /// 修改用户信息失败
    public const int UpdateUserFailed = 1219;
}