using SimpleAdmin.WebApi.Api.Implements;
using SimpleAdmin.WebApi.DataContracts.Dto.Security;
using SimpleAdmin.WebApi.Infrastructure.Utils;

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
    ///     发送短信数字码
    /// </summary>
    /// <param name="smsSender"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<SendSmsCodeRsp> SendSmsCode(ISmsSender smsSender, SendSmsCodeReq req);


    /// <summary>
    ///     完成图片验证
    /// </summary>
    /// <returns></returns>
    Task<bool> VerifyCaptcha(VerifyCaptchaReq req);
}