using SimpleAdmin.WebApi.DataContracts.Dto.Account;

namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     帐号接口
/// </summary>
public interface IAccountApi
{
    /// <summary>
    ///     创建帐号
    /// </summary>
    void Create(CreateReq req);
}
