using SimpleAdmin.DataContract.DataTransferObjects.Permission;

namespace SimpleAdmin.Rest.Main.Api.Permission;

public interface IPermissionApi
{
    Task<UserMenuRsp> GetMyMenu();
}