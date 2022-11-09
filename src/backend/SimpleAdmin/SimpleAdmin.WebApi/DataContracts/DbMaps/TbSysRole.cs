using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps;

/// <summary>
///     角色表
/// </summary>
[Table]
public record TbSysRole : DefaultTable
{
    /// <summary>
    ///     角色名称
    /// </summary>
    [Column]
    public string RoleName { get; set; }
}