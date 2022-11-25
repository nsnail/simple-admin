using SimpleAdmin.WebApi.DataContracts.Dto;
using SimpleAdmin.WebApi.DataContracts.Dto.Department;

namespace SimpleAdmin.WebApi.Api.Sys;

/// <summary>
///     部门接口
/// </summary>
public interface IDepartmentApi
{
    /// <summary>
    ///     分页获取部门列表
    /// </summary>
    /// <returns></returns>
    Task<List<QueryDepartmentsRsp>> List(PagedListReq<QueryDepartmentsReq> req);
}