using Furion.DataEncryption;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SimpleAdmin.DataContract.DataTransferObjects.Auth;
using SimpleAdmin.Rest.Core;

namespace SimpleAdmin.Rest.Entry.Api.Implements;

/// <inheritdoc cref="IAccountApi" />
public class AccountApi : RestBase<AccountApi>, IAccountApi
{
    /// <inheritdoc />
    public AccountApi(ILogger<AccountApi> logger, IMediator mediator) : base(logger, mediator)
    { }


    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<LoginRsp> Login(LoginReq cmd)
    {
        return await Mediator.Send(cmd);
    }

    /// <inheritdoc />
    public async Task<RegisterRsp> Register(RegisterReq req)
    {
        return await Mediator.Send(req);
    }


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