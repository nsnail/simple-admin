using SimpleAdmin.DataContract.DataTransferObjects.Auth;

namespace SimpleAdmin.Rest.Main.Api.Auth;

/// <summary>
///     帐号接口
/// </summary>
public interface ITokenApi
{
    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LoginRsp> Login(LoginReq req);
}