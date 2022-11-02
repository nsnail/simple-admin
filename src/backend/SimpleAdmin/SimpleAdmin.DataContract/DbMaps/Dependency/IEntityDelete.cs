using System.ComponentModel;
using FreeSql.DataAnnotations;

namespace SimpleAdmin.DataContract.DbMaps.Dependency;

/// <summary>
///     删除接口
/// </summary>
public interface IEntityDelete
{
    /// <summary>
    ///     是否删除
    /// </summary>

    [Description("是否删除")]
    [Column(Position = -40)]
    bool IsDeleted { get; set; }
}