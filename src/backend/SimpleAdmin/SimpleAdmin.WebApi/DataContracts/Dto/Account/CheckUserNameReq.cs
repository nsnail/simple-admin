using SimpleAdmin.WebApi.Aop.Attributes;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     创建用户请求
/// </summary>
public record CheckUserNameReq : DataContract
{
    /// <summary>
    ///     用户名
    /// </summary>
    [RequiredField]
    [RegularExpression(Strings.REGEX_USERNAME, ErrorMessage = Strings.MSG_USERNAME_STRONG)]
    public string UserName { get; set; }
}