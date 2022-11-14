using SimpleAdmin.WebApi.Api.Implements;
using SimpleAdmin.WebApi.DataContracts.Dto.Security;

namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     安全接口
/// </summary>
public interface ISecurityApi
{
    /// <summary>
    ///     获取验证图片
    /// </summary>
    /// <returns></returns>
    Task<GetCaptchaRsp> GetCaptchaImage();


    /// <summary>
    ///     发送短信验证码
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<SendSmsCodeRsp> SendSmsCode(SendSmsCodeReq req);


    /// <summary>
    ///     检查验证信息
    /// </summary>
    /// <returns></returns>
    Task<bool> VerifyCaptcha(VerifyCaptchaReq req);
}
