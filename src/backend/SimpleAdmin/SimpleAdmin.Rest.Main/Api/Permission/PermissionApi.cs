using MediatR;
using SimpleAdmin.DataContract.DataTransferObjects.Permission;

namespace SimpleAdmin.Rest.Main.Api.Permission;

public class PermissionApi : ApiBase<PermissionApi>, IPermissionApi
{
    /// <inheritdoc />
    public PermissionApi(ILogger<PermissionApi> logger, IMediator mediator) : base(logger, mediator)
    { }

    /// <inheritdoc />
    public Task<UserMenuRsp> GetMyMenu()
    {
        throw new NotImplementedException();
    }
}