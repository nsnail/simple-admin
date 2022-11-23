using SimpleAdmin.WebApi.Aop.Attributes;
using SimpleAdmin.WebApi.DataContracts.Dto.Account;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     发送短信验证码请求
/// </summary>
public record SendSmsCodeReq : CheckMobileReq
{
    /// <summary>
    ///     类型
    /// </summary>
    [RequiredField]
    public Enums.SmsCodeTypes Type { get; set; }


    /// <summary>
    ///     人机校验请求
    /// </summary>
    [RequiredField]
    public VerifyCaptchaReq VerifyCaptchaReq { get; set; }
}