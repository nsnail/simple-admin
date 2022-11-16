using System.ComponentModel.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.Dto.Security;
using SimpleAdmin.WebApi.Infrastructure.Constants;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     创建用户请求
/// </summary>
public record CreateReq : AccountInfo
{
    /// <summary>
    ///     密码
    /// </summary>
    [Required(ErrorMessage = Strings.RULE_REQUIRED)]
    [RegularExpression(Strings.REGEX_PASSWORD, ErrorMessage = Strings.RULE_PASSWORD)]
    public string Password { get; set; }

    /// <inheritdoc cref="AccountInfo.UserName" />
    [Required(ErrorMessage = Strings.RULE_REQUIRED)]
    public override string UserName { get; set; }


    /// <summary>
    ///     短信验证码信息
    /// </summary>
    [Required]
    public VerifySmsCodeReq VerifySmsCodeReq { get; set; }
}