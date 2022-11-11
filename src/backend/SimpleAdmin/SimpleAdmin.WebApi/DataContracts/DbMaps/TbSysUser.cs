using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps;

/// <summary>
///     用户表
/// </summary>
[Table]
public record TbSysUser : DefaultTable
{
    /// <summary>
    ///     手机号
    /// </summary>
    [Column]
    public long? Mobile { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [Column]
    public string Password { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    [Column]
    public string UserName { get; set; }
}