namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     验证码图像信息
/// </summary>
public record CaptchaImageRsp : CaptchaImageInfo
{
    /// <summary>
    ///     token
    /// </summary>
    public string Token { get; set; }
}