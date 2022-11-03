using Furion.DataEncryption;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAdmin.Application.Services.Auth;
using SimpleAdmin.DataContract.DataTransferObjects.Auth;
using SimpleAdmin.Infrastructure.Attributes;
using SimpleAdmin.WebApi.AopHooks;

namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     帐号Api
/// </summary>
public interface IAccountApi
{
    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    Task<LoginRsp> Login(LoginReq cmd);


    /// <summary>
    ///     注册
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<RegisterRsp> Register(RegisterReq req);


    /// <summary>
    ///     获取Token
    /// </summary>
    /// <returns></returns>
    Task<LoginRsp> GenToken();
}

/// <inheritdoc cref="SimpleAdmin.WebApi.Api.IAccountApi" />
public class AccountApi : ApiBase<AccountApi>, IAccountApi
{

    private readonly IAccountService _accountService;
    /// <inheritdoc />
    public AccountApi(ILogger<AccountApi> logger, IAccountService accountService) : base(logger)
    {
        _accountService = accountService;
    }


    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<LoginRsp> Login(LoginReq cmd)
    {
        return new LoginRsp();
    }

    /// <inheritdoc />
    [AllowAnonymous]
    [ServiceFilter(typeof(TransactionInterceptor))]
    public async Task<RegisterRsp> Register(RegisterReq req)
    {
        var ret= (await _accountService.Register(req.UserName)).Adapt<RegisterRsp>();
        return ret;
    }


    /// <inheritdoc />
    [AllowAnonymous]
    public Task<LoginRsp> GenToken()
    {
        // token
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object> {
            { "UserId", "userid" },  // 存储Id
            { "Account", "account" } // 存储用户名
        });
        return Task.FromResult(new LoginRsp {
            Token = accessToken
        });
    }
}