using Mapster;
using Microsoft.AspNetCore.Authorization;
using SimpleAdmin.WebApi.DataContracts.DbMaps;
using SimpleAdmin.WebApi.DataContracts.Dto;
using SimpleAdmin.WebApi.DataContracts.Dto.Department;

namespace SimpleAdmin.WebApi.Api.Implements;

/// <inheritdoc cref="SimpleAdmin.WebApi.Api.IDepartmentApi" />
public class DepartmentApi : ApiBase<IDepartmentApi, TbSysDepartment>, IDepartmentApi
{
    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<List<QueryDepartmentsRsp>> List(PagedListReq<QueryDepartmentsReq> req)
    {
        var ret = (await Repository.Select.ToTreeListAsync()).ConvertAll(x => x.Adapt<QueryDepartmentsRsp>());
        return ret;
    }
}