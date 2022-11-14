namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     获取验证图片响应
/// </summary>
public record GetCaptchaRsp : CaptchaInfo
{
    /// <summary>
    ///     背景图（base64）
    /// </summary>
    public virtual string BackgroundImage { get; set; }

    /// <summary>
    ///     滑块图（base64）
    /// </summary>
    public virtual string SliderImage { get; set; }
}