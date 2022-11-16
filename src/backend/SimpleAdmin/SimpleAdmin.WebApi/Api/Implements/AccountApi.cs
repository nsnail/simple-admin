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
    public async Task<bool> CheckUserName(CheckUserNameReq req)
    {
        return !await _accountRepository.Select.AnyAsync(a => a.UserName == req.UserName);
    }

    /// <inheritdoc />
    [AllowAnonymous]
    public async Task Create(CreateReq req)
    {
        //短信验证码
        var checkResult = await _securityApi.VerifySmsCode(req.VerifySmsCodeReq);
        if (!checkResult) throw Oops.Oh(Enums.ErrorCodes.InvalidInput, "短信验证码不正确");

        var tbUser = new TbSysUser {
            Mobile   = req.VerifySmsCodeReq.Mobile.Int64(),
            SaltCode = Guid.NewGuid(),
            UserName = req.UserName
        };
        tbUser.Password = req.Password.Password(tbUser.SaltCode.ToString());
        await _accountRepository.InsertAsync(tbUser);
    }
}
