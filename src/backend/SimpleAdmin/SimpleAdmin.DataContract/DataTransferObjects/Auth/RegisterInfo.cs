namespace SimpleAdmin.DataContract.DataTransferObjects.Auth;

/// <summary>
///     注册信息
/// </summary>
public record RegisterInfo : DtoAbstraction
{
    /// <summary>
    ///     用户名
    /// </summary>
    public string UserName { get; set; }
}