using Microsoft.AspNetCore.Authorization;
using NSExt.Extensions;
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
public class AccountApi : ApiBase<IAccountApi>, IAccountApi
{
    /// <inheritdoc />
    [AllowAnonymous]
    public void Create(CreateReq req)
    {
        _accountRepository.Insert(new TbSysUser {
            UserName = req.UserName
        });
        Logger.Info($"当前线程：{Thread.CurrentThread.ManagedThreadId}");
    }

    private readonly AccountRepository _accountRepository;

    /// <inheritdoc />
    public AccountApi(ILogger<IAccountApi> logger, AccountRepository accountRepository) : base(logger)
    {
        _accountRepository = accountRepository;
    }
}