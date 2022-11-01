namespace SimpleAdmin.DataContract.DataTransferObjects.Auth;

/// <summary>
///     登录信息
/// </summary>
public record LoginInfo : DtoAbstraction
{
    /// <summary>
    ///     密码
    /// </summary>
    public virtual string Password { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    public virtual string UserName { get; set; }
}