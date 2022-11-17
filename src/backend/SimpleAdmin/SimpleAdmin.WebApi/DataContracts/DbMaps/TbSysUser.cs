using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps;

/// <summary>
///     用户表
/// </summary>
[Table]
public record TbSysUser : DefaultTable, IFieldBitSet
{
    /// <inheritdoc />
    public long BitSet { get; set; }

    /// <summary>
    ///     手机号
    /// </summary>
    [Column]
    public long? Mobile { get; init; }

    /// <summary>
    ///     密码
    /// </summary>
    [Column]
    public Guid Password { get; set; }


    /// <summary>
    ///     做授权验证的Token，全局唯一，可以随时重置（强制下线）
    /// </summary>
    [Column]
    public Guid Token { get; init; }


    /// <summary>
    ///     用户名
    /// </summary>
    [Column]
    public string UserName { get; init; }
}