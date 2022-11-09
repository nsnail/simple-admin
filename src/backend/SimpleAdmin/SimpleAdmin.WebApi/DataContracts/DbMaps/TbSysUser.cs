using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps;

/// <summary>
///     用户表
/// </summary>
public record TbSysUser : DefaultTable
{
    /// <summary>
    ///     用户名
    /// </summary>
    public string UserName { get; set; }
}