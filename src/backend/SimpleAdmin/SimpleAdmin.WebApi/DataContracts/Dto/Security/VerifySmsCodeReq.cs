using SimpleAdmin.WebApi.Aop.Attributes;
using SimpleAdmin.WebApi.DataContracts.Dto.Account;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     核实短信验证码请求
/// </summary>
public record VerifySmsCodeReq : CheckMobileReq
{
    /// <summary>
    ///     验证码
    /// </summary>
    [RequiredField]
    [RegularExpression(Strings.REGEX_SMSCODE, ErrorMessage = Strings.MSG_SMSCODE_NUMBER)]
    public string Code { get; set; }
}