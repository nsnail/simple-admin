namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     获取验证图片响应
/// </summary>
public record GetCaptchaRsp : ICacheKey
{
    /// <summary>
    ///     背景图（base64）
    /// </summary>
    public virtual string BackgroundImage { get; set; }

    /// <inheritdoc />
    public string CacheKey { get; set; }

    /// <summary>
    ///     滑块图（base64）
    /// </summary>
    public virtual string SliderImage { get; set; }
}