using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;

namespace SimpleAdmin.DataContract.DbMaps.Dependency;

/// <summary>
///     修改接口
/// </summary>
public interface IEntityUpdate
{
    /// <summary>
    ///     修改时间
    /// </summary>

    [Description("修改时间")]
    [Column(Position = -10, CanInsert = false, ServerTime = DateTimeKind.Local)]
    DateTime? ModifiedTime { get; set; }

    /// <summary>
    ///     修改者Id
    /// </summary>

    [Description("修改者Id")]
    [Column(Position = -12, CanInsert = false)]
    long? ModifiedUserId { get; set; }

    /// <summary>
    ///     修改者
    /// </summary>
    [Description("修改者")]
    [Column(Position = -11, CanInsert = false)]
    [MaxLength(50)]
    string ModifiedUserName { get; set; }
}