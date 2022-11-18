using System.Text.Json.Serialization;
using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps;

/// <summary>
///     用户表
/// </summary>
[Table]
public record TbSysUser : FullTable, IFieldBitSet
{
    /// <inheritdoc />
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual long BitSet { get; set; }

    /// <summary>
    ///     手机号
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual long? Mobile { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual Guid Password { get; set; }


    /// <summary>
    ///     做授权验证的Token，全局唯一，可以随时重置（强制下线）
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual Guid Token { get; set; }


    /// <summary>
    ///     用户名
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual string UserName { get; set; }
}