using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using NSExt.Extensions;
using SimpleAdmin.WebApi.DataContracts.DbMaps;
using SimpleAdmin.WebApi.DataContracts.Dto.Account;
using SimpleAdmin.WebApi.Infrastructure.Constant;
using SimpleAdmin.WebApi.Repositories;

namespace SimpleAdmin.WebApi.Api.Implements;

/// <inheritdoc cref="IAccountApi" />
public class AccountApi : ApiBase<IAccountApi>, IAccountApi
{
    /// <inheritdoc />
    public AccountApi(AccountRepository accountRepository, ISecurityApi securityApi)
    {
        _accountRepository = accountRepository;
        _securityApi       = securityApi;
    }

    private readonly AccountRepository _accountRepository;

    private readonly ISecurityApi _securityApi;


    /// <inheritdoc />
    [AllowAnonymous]
    public bool CheckUserName(CheckUserNameReq req)
    {
        return !_accountRepository.Select.Any(a => a.UserName == req.UserName);
    }

    /// <inheritdoc />
    [AllowAnonymous]
    public async Task Create(CreateReq req)
    {
        //短信验证码
        var sdfff = await _securityApi.VerifySmsCode(req.VerifySmsCodeReq);
        if (!sdfff) throw Oops.Oh(Enums.ErrorCodes.InvalidInput, "短信验证码不正确");

        await _accountRepository.InsertAsync(new TbSysUser {
            Mobile   = req.VerifySmsCodeReq.Mobile.Int64(),
            Password = req.Password,
            UserName = req.UserName
        });
        Logger.Info($"当前线程：{Thread.CurrentThread.ManagedThreadId}");
    }
}