using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;

namespace SimpleAdmin.DataContract.DbMaps.Dependency;

/// <summary>
///     添加接口
/// </summary>
public interface IEntityAdd
{
    /// <summary>
    ///     创建时间
    /// </summary>
    [Description("创建时间")]
    [Column(Position = -20, CanUpdate = false, ServerTime = DateTimeKind.Local)]
    DateTime? CreatedTime { get; set; }

    /// <summary>
    ///     创建者用户Id
    /// </summary>
    [Description("创建者Id")]
    [Column(Position = -22, CanUpdate = false)]
    long? CreatedUserId { get; set; }

    /// <summary>
    ///     创建者
    /// </summary>
    [Description("创建者")]
    [Column(Position = -21, CanUpdate = false)]
    [MaxLength(50)]
    string CreatedUserName { get; set; }
}