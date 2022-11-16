using System.ComponentModel.DataAnnotations;
using SimpleAdmin.WebApi.Infrastructure.Constant;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     帐号信息
/// </summary>
public record AccountInfo : DtoBase
{
    /// <summary>
    ///     用户名
    /// </summary>
    [RegularExpression(Strings.REGEX_USERNAME, ErrorMessage = Strings.RULE_USERNAME)]
    public virtual string UserName { get; set; }
}