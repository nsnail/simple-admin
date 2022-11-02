using SimpleAdmin.DataContract.DataTransferObjects.Permission;

namespace SimpleAdmin.Rest.Entry.Api;

public interface IPermissionApi
{
    Task<UserMenuRsp> GetMyMenu();
}