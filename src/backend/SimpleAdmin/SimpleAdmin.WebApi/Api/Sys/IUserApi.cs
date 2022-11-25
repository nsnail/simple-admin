using SimpleAdmin.WebApi.DataContracts.Dto;
using SimpleAdmin.WebApi.DataContracts.Dto.User;

namespace SimpleAdmin.WebApi.Api.Sys;

/// <summary>
///     用户接口
/// </summary>
public interface IUserApi
{
    /// <summary>
    ///     获取个人信息
    /// </summary>
    /// <returns></returns>
    Task<GetProfileRsp> GetProfile();

    /// <summary>
    ///     分页获取用户列表
    /// </summary>
    /// <returns></returns>
    Task<PagedListRsp<UserRsp>> QueryUsers(PagedListReq<QueryUsersReq> req);
}