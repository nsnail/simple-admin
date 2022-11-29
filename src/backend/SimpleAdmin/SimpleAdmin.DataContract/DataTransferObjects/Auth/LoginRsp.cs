namespace SimpleAdmin.DataContract.DataTransferObjects.Auth;

/// <summary>
///     登录响应
/// </summary>
public record LoginRsp : LoginInfo
{
    /// <summary>
    ///     Token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    ///     用户信息
    /// </summary>
    public dynamic UserInfo { get; set; }
}