using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAdmin.WebApi.DataContracts.DbMaps;
using SimpleAdmin.WebApi.DataContracts.Dto;
using SimpleAdmin.WebApi.DataContracts.Dto.User;
using SimpleAdmin.WebApi.Repositories;

namespace SimpleAdmin.WebApi.Api.Sys.Implements;

/// <inheritdoc cref="IUserApi" />
public class UserApi : ApiBase<IUserApi, TbSysUser>, IUserApi
{
    /// <inheritdoc />
    public UserApi(Repository<TbSysMenu> repMenu)
    {
        _repMenu = repMenu;
    }

    private readonly Repository<TbSysMenu> _repMenu;

    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<GetProfileRsp> GetProfile()
    {
        var ret = new GetProfileRsp {
            Menu = (await _repMenu.Select.ToTreeListAsync()).ConvertAll(x => x.Adapt<GetProfileRsp.MenuInfo>()),
            Permissions = new List<string> {
                "list.add",
                "list.edit",
                "list.delete",
                "user.add",
                "user.edit",
                "user.delete"
            }
        };

        return ret;
    }


    /// <inheritdoc />
    [AllowAnonymous]
    [HttpPost]
    public async Task<PagedListRsp<UserRsp>> QueryUsers(PagedListReq<QueryUsersReq> req)
    {
        var ret = await Repository.GetPagedListAsync(req.DynamicFilter, req.Page, req.PageSize);
        return new PagedListRsp<UserRsp> {
            Page     = req.Page,
            PageSize = req.PageSize,
            Rows = ret.list.ConvertAll(x => new UserRsp {
                UserName = x.UserName
            }),
            Total = ret.total
        };
    }
}