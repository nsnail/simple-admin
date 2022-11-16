using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SimpleAdmin.WebApi.Infrastructure.Constant;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     发送短信验证码请求
/// </summary>
public record SendSmsCodeReq : SmsCodeInfo
{
    /// <inheritdoc />
    [JsonIgnore]
    public override string Code { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public override DateTime CreateTime { get; set; }

    /// <inheritdoc cref="SmsCodeInfo.Mobile" />
    [Required]
    public override string Mobile { get; set; }

    /// <inheritdoc cref="SmsCodeInfo.Type" />
    [Required]
    public override Enums.SmsCodeTypes Type { get; set; }

    /// <summary>
    ///     人机校验请求
    /// </summary>
    [Required]
    public VerifyCaptchaReq VerifyCaptchaReq { get; set; }
}