using System.ComponentModel.DataAnnotations;
using SimpleAdmin.WebApi.Infrastructure.Constants;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     创建用户请求
/// </summary>
public record CheckUserNameReq : AccountInfo
{
    /// <inheritdoc cref="AccountInfo.UserName" />
    [Required(ErrorMessage = Strings.RULE_REQUIRED)]
    public override string UserName { get; set; }
}