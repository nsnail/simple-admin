using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SimpleAdmin.DataContract.DataTransferObjects.Auth;

/// <summary>
///     登录请求
/// </summary>
/// <inheritdoc cref="LoginInfo" />
public record LoginReq : LoginInfo, ICommand, IRequest<LoginRsp>
{
    /// <inheritdoc />
    [Required]
    public override string Password { get; set; }


    /// <inheritdoc />
    [Required]
    public override string UserName { get; set; }
}