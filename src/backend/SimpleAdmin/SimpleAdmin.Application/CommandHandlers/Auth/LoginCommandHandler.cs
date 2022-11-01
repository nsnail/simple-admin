using MediatR;
using SimpleAdmin.DataContract.DataTransferObjects.Auth;

namespace SimpleAdmin.Application.CommandHandlers.Auth;

public class LoginCommandHandler : IRequestHandler<LoginReq, LoginRsp>
{
    /// <inheritdoc />
    public Task<LoginRsp> Handle(LoginReq request, CancellationToken cancellationToken)
    {
        var userInfo = new {
            UserId    = "1",
            UserName  = "Administrator",
            Dashboard = "0",
            Role = new[] {
                "SA",
                "admin",
                "Auditor"
            }
        };
        return Task.FromResult(new LoginRsp {
            Password = request.Password,
            UserName = request.UserName,
            Token = JWTEncryption.Encrypt(new Dictionary<string, object> {
                { nameof(LoginRsp.UserInfo), userInfo }
            }),
            UserInfo = userInfo
        });
    }
}