using Furion.DataEncryption;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using NSExt.Extensions;
using SimpleAdmin.WebApi.DataContracts;
using SimpleAdmin.WebApi.DataContracts.DbMaps;
using SimpleAdmin.WebApi.DataContracts.Dto.Account;
using SimpleAdmin.WebApi.Infrastructure.Constant;
using SimpleAdmin.WebApi.Repositories;

namespace SimpleAdmin.WebApi.Api.Implements;

/// <inheritdoc cref="IAccountApi" />
public class AccountApi : ApiBase<IAccountApi>, IAccountApi
{
    /// <inheritdoc />
    public AccountApi(AccountRepository rep, ISecurityApi securityApi)
    {
        _rep         = rep;
        _securityApi = securityApi;
    }

    private readonly AccountRepository _rep;

    private readonly ISecurityApi _securityApi;


    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<bool> CheckUserName(CheckUserNameReq req)
    {
        return !await _rep.Select.AnyAsync(a => a.UserName == req.UserName);
    }


    /// <inheritdoc />
    [AllowAnonymous]
    public async Task Create(CreateReq req)
    {
        //短信验证码
        var checkResult = await _securityApi.VerifySmsCode(req.VerifySmsCodeReq);
        if (!checkResult) throw Oops.Oh(Enums.ErrorCodes.InvalidInput, Strings.MSG_SMSCODE_WRONG);

        var tbUser = new TbSysUser {
            Mobile   = req.VerifySmsCodeReq.Mobile.Int64(),
            Token    = Guid.NewGuid(),
            UserName = req.UserName,
            Password = req.Password.Pwd().Guid()
        };
        await _rep.InsertAsync(tbUser);
    }

    /// <inheritdoc />
    [AllowAnonymous]
    public async Task Login(LoginReq req)
    {
        var tbUser = await _rep.GetAsync(x => x.UserName == req.UserName && x.Password == req.Password.Pwd().Guid());
        if (tbUser is null) throw Oops.Oh(Enums.ErrorCodes.InvalidInput, Strings.MSG_UNAME_PASSWORD_WRONG);

        if (tbUser.BitSet.HasFlag(Enums.UserBitSets.Disabled))
            throw Oops.Oh(Enums.ErrorCodes.InvalidInput, Strings.MSG_USER_DISABLED);


        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object> {
            {
                nameof(ContextUser), new ContextUser {
                    Id       = tbUser.Id,
                    UserName = tbUser.UserName
                }
            }
        });


        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

        // 设置响应报文头
        App.HttpContext.Response.Headers["access-token"]   = accessToken;
        App.HttpContext.Response.Headers["x-access-token"] = refreshToken;
    }
}