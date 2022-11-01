using Furion.DataEncryption;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SimpleAdmin.DataContract.DataTransferObjects.Auth;

namespace SimpleAdmin.Rest.Main.Api.Auth;

/// <inheritdoc cref="ITokenApi" />
public class TokenApi : ApiBase<TokenApi>, ITokenApi
{
    /// <inheritdoc />
    public TokenApi(ILogger<TokenApi> logger, IMediator mediator) : base(logger, mediator)
    { }


    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<LoginRsp> Login(LoginReq cmd)
    {
        return await Mediator.Send(cmd);
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