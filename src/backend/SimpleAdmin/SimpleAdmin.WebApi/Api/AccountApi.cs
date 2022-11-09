using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using SimpleAdmin.WebApi.DataContracts.DbMaps;
using SimpleAdmin.WebApi.DataContracts.Dto.Account;
using SimpleAdmin.WebApi.Repositories;

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

/// <inheritdoc cref="SimpleAdmin.WebApi.Api.IAccountApi" />
public class AccountApi : IAccountApi, IDynamicApiController
{
    private readonly AccountRepository _accountRepository;

    /// <param name="accountRepository"></param>
    public AccountApi(AccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    /// <inheritdoc />
    [AllowAnonymous]
    public void Create(CreateReq req)
    {
        _accountRepository.Insert(new TbSysUser {
            UserName = req.UserName
        });
    }
}