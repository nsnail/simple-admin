using SimpleAdmin.DataContract.DataTransferObjects.Auth;

namespace SimpleAdmin.Rest.Entry.Api;

/// <summary>
///     帐号接口
/// </summary>
public interface IAccountApi
{
    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LoginRsp> Login(LoginReq req);

    /// <summary>
    ///     注册
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<RegisterRsp> Register(RegisterReq req);
}