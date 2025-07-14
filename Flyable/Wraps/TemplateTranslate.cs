namespace Flyable.Wraps;

public enum ActionType
{
    Register,
    Unregister,
    LongTimeLogin,
    ModifySensitiveInfo,
    ModifyEmail,
}
public readonly struct TemplateTranslate(ActionType type, string username, string code)
{
#if DEBUG
    private string UnknownKeyword => $"未知操作{type}";
#else
    private const string UnknownKeyword => "未知操作";
#endif
    public string Template => $"""
                               <h1>Flyable系统通知</h1>
                               <p>
                               {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                               </p>
                               <p>
                               尊敬的{username}，您好，感谢使用Flyable。
                               您当前正在执行的操作是
                               <font color=“#0CC0FF”>
                               <i><b>{type switch
    {
        ActionType.Register => "使用此邮箱注册新账户",
        ActionType.Unregister => "注销与此邮箱绑定的Flyable账户",
        ActionType.LongTimeLogin => "长时间未登录账户登录Flyable",
        ActionType.ModifySensitiveInfo => "修改与此邮箱相关联的账户的敏感信息",
        ActionType.ModifyEmail => "修改与此邮箱相关联的账户的邮箱",
        var _ => UnknownKeyword
    }}</b></i></font>，
                               操作确认验证码为{code}。
                               </p>
                               <p>
                               请注意：验证码的有效期为5分钟，请尽快完成操作。
                               </p>
                               <p>
                               如非本人操作，请{type switch
    {
        ActionType.Register => "忽略此邮件",
        var _ => "及时修改密码"
    }}。
                               </p>
                               """;
}