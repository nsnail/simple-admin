using MediatR;
using SimpleAdmin.DataContract.DataTransferObjects.Permission;
using SimpleAdmin.Rest.Core;

namespace SimpleAdmin.Rest.Entry.Api.Implements;

public class PermissionApi : RestBase<PermissionApi>, IPermissionApi
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