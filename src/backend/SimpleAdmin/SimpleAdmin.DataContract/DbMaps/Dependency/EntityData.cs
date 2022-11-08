using System.ComponentModel;
using FreeSql.DataAnnotations;

namespace SimpleAdmin.DataContract.DbMaps.Dependency;

/// <summary>
///     实体数据权限
/// </summary>
public record EntityData : EntityBase, IData
{
    /// <summary>
    ///     拥有者Id
    /// </summary>
    [Description("拥有者Id")]
    [Column(Position = -41)]
    public long? OwnerId { get; set; }

    /// <summary>
    ///     拥有者部门Id
    /// </summary>
    [Description("拥有者部门Id")]
    [Column(Position = -40)]
    public long? OwnerOrgId { get; set; }
}