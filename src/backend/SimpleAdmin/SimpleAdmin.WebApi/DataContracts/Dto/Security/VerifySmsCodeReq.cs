using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SimpleAdmin.WebApi.Infrastructure.Constants;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     短信验证码信息
/// </summary>
public record VerifySmsCodeReq : SmsCodeInfo
{
    /// <inheritdoc cref="SmsCodeInfo.Code" />
    [Required]
    public override string Code { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public override DateTime CreateTime { get; set; }

    /// <inheritdoc cref="SmsCodeInfo.Mobile" />
    [Required]
    public override string Mobile { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public override Enums.SmsCodeTypes Type { get; set; }
}