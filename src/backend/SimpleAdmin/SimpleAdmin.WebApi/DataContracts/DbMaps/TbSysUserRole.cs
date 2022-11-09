using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps;

/// <summary>
///     用户与角色映射表
/// </summary>
[Table]
public record TbSysUserRole : ImmutableTable
{
    /// <summary>
    ///     角色id
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    ///     用户id
    /// </summary>
    public long UserId { get; set; }
}