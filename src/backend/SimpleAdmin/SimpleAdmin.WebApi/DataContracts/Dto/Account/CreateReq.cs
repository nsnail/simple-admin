using System.ComponentModel.DataAnnotations;
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

    /// <inheritdoc />
    [Required(ErrorMessage = Strings.RULE_REQUIRED)]
    [RegularExpression(Strings.REGEX_USERNAME, ErrorMessage = Strings.RULE_USERNAME)]
    public override string UserName { get; set; }
}