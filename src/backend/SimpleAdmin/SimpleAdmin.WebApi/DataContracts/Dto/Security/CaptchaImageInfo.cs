namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///    验证码图像信息
/// </summary>
public record CaptchaImageInfo : DtoBase
{
    /// <summary>
    ///     背景图（base64）
    /// </summary>
    public virtual string BackgrondImage { get; set; }

    /// <summary>
    ///     滑块图（base64）
    /// </summary>
    public virtual string SliderImage { get; set; }
}