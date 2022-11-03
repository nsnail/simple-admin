
using SimpleAdmin.DataContract.DataTransferObjects.Permission;


namespace SimpleAdmin.WebApi.Api;


/// <summary>
/// 权限Api
/// </summary>
public interface IPermissionApi
{

    /// <summary>
    /// 获取我的菜单
    /// </summary>
    /// <returns></returns>
    Task<UserMenuRsp> GetMyMenu();


}

/// <inheritdoc cref="SimpleAdmin.WebApi.Api.IPermissionApi" />
public class PermissionApi : ApiBase<PermissionApi>, IPermissionApi
{
    /// <inheritdoc />
    public PermissionApi(ILogger<PermissionApi> logger) : base(logger)
    { }

    /// <inheritdoc />
    public Task<UserMenuRsp> GetMyMenu()
    {
        throw new NotImplementedException();
    }
}