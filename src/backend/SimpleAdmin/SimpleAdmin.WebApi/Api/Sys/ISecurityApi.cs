using SimpleAdmin.WebApi.DataContracts.Dto.Security;
using SimpleAdmin.WebApi.Infrastructure.Utils;

namespace SimpleAdmin.WebApi.Api.Sys;

/// <summary>
///     安全接口
/// </summary>
public interface ISecurityApi
{
    /// <summary>
    ///     获取人机校验图
    /// </summary>
    /// <returns></returns>
    Task<GetCaptchaRsp> GetCaptchaImage();


    /// <summary>
    ///     发送短信验证码
    /// </summary>
    /// <param name="smsSender"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task SendSmsCode(ISmsSender smsSender, SendSmsCodeReq req);


    /// <summary>
    ///     完成人机校验
    /// </summary>
    /// <returns></returns>
    Task<bool> VerifyCaptcha(VerifyCaptchaReq req);

    /// <summary>
    ///     完成短信验证
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<bool> VerifySmsCode(VerifySmsCodeReq req);
}