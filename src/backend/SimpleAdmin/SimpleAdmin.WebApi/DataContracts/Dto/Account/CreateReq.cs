using System.ComponentModel.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.Dto.Security;
using SimpleAdmin.WebApi.Infrastructure.Constant;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     创建用户请求
/// </summary>
public record CreateReq : AccountInfo
{
    /// <inheritdoc cref="AccountInfo.Password" />
    [Required(ErrorMessage = Strings.MSG_REQUIRED)]
    public override string Password { get; set; }

    /// <inheritdoc cref="AccountInfo.UserName" />
    [Required(ErrorMessage = Strings.MSG_REQUIRED)]
    public override string UserName { get; set; }


    /// <summary>
    ///     短信验证码信息
    /// </summary>
    [Required]
    public VerifySmsCodeReq VerifySmsCodeReq { get; set; }
}