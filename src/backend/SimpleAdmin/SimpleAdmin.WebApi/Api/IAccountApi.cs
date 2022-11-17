using SimpleAdmin.WebApi.DataContracts.Dto.Account;

namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     帐号接口
/// </summary>
public interface IAccountApi
{
    /// <summary>
    ///     检查用户名可用性
    /// </summary>
    Task<bool> CheckUserName(CheckUserNameReq req);

    /// <summary>
    ///     创建帐号
    /// </summary>
    Task Create(CreateReq req);


    /// <summary>
    ///     帐号登录
    /// </summary>
    Task Login(LoginReq req);
}