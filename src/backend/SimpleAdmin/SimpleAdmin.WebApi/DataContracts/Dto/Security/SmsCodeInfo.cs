using SimpleAdmin.WebApi.DataContracts.Dto.Account;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     发送短信验证码响应
/// </summary>
public record SmsCodeInfo : CheckMobileReq
{
    /// <summary>
    ///     验证码
    /// </summary
    public string Code { get; set; }


    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}