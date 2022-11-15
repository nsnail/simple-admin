using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SimpleAdmin.WebApi.Infrastructure.Constants;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     发送短信数字码请求
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
    [RegularExpression(Strings.REGEX_MOBILE, ErrorMessage = Strings.RULE_MOBILE)]
    public override string Mobile { get; set; }

    /// <inheritdoc />
    [Required]
    [EnumDataType(typeof(OperationTypes))]
    public override OperationTypes OperationType { get; set; }

    /// <summary>
    ///     检查验证信息请求
    /// </summary>
    [Required]
    public VerifyCaptchaReq VerifyCaptchaReq { get; set; }
}