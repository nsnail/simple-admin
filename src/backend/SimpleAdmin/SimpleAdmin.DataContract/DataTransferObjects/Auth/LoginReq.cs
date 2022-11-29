using System.ComponentModel.DataAnnotations;

namespace SimpleAdmin.DataContract.DataTransferObjects.Auth;

/// <summary>
///     登录请求
/// </summary>
/// <inheritdoc cref="LoginInfo" />
public record LoginReq : LoginInfo
{
    /// <inheritdoc />
    [Required]
    public override string Password { get; set; }


    /// <inheritdoc />
    [Required]
    public override string UserName { get; set; }
}