using System.ComponentModel.DataAnnotations;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
/// 检查验证信息请求
/// </summary>
public record VerifyCaptchaReq : CaptchaInfo
{
    /// <summary>
    ///     验证数据
    /// </summary>
    [Required]
    public string VerifyData { get; set; }

    /// <inheritdoc />
    [Required]
    public override string Token { get; set; }


}