using Microsoft.AspNetCore.Authorization;
using NSExt.Extensions;
using SimpleAdmin.WebApi.DataContracts.DbMaps;
using SimpleAdmin.WebApi.DataContracts.Dto.Account;
using SimpleAdmin.WebApi.Repositories;

namespace SimpleAdmin.WebApi.Api.Implements;

/// <inheritdoc cref="IAccountApi" />
public class AccountApi : ApiBase<IAccountApi>, IAccountApi
{
    /// <inheritdoc />
    public AccountApi(AccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    private readonly AccountRepository _accountRepository;

    /// <inheritdoc />
    [AllowAnonymous]
    public void Create(CreateReq req)
    {
        _accountRepository.Insert(new TbSysUser {
            UserName = req.UserName
        });
        Logger.Info($"当前线程：{Thread.CurrentThread.ManagedThreadId}");
    }
}
