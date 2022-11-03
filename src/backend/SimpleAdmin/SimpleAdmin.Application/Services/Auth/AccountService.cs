using Microsoft.AspNetCore.Mvc;
using SimpleAdmin.Application.Repositories.Auth;
using SimpleAdmin.DataContract.DbMaps;
using SimpleAdmin.Infrastructure.Attributes;

namespace SimpleAdmin.Application.Services.Auth;


public interface IAccountService
{
    /// <summary>
    /// </summary>
    /// <param name="userName"></param>
    /// <returns>
    ///     <returnref name="userId">!!!</returnref>
    /// </returns>
    Task<TbSysUser> Register(string userName);
}

public class AccountService : IAccountService, IScoped
{
    private readonly AccountRepository _accountRepository;

    public AccountService(AccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    /// <inheritdoc />
    public async Task<TbSysUser> Register(string userName)
    {
        var ret = await _accountRepository.InsertAsync(new TbSysUser {
            UserName = userName
        });
        return ret;
    }
}