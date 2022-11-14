namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     验证码信息
/// </summary>
public record CaptchaInfo : DtoBase
{

    /// <summary>
    ///     识别编码
    /// </summary>
    public virtual string Token { get; set; }
}