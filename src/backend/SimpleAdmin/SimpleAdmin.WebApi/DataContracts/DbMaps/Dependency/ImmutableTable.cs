using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.Aop.Attributes;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

/// <summary>
///     数据库默认表基类
/// </summary>
public abstract record ImmutableTable : ITable, IFieldPrimary, IFieldAdd, IFieldDelete
{
    /// <inheritdoc />
    [Description("创建时间")]
    [Column(CanUpdate = false, ServerTime = DateTimeKind.Local)]
    public DateTime CreatedTime { get; set; }

    /// <inheritdoc />
    [Description("创建者Id")]
    [Column(CanUpdate = false)]
    public long? CreatedUserId { get; set; }

    /// <inheritdoc />
    [Description("创建者")]
    [Column(CanUpdate = false)]
    [MaxLength(50)]
    public string CreatedUserName { get; set; }


    /// <inheritdoc />
    [Description("主键Id")]
    [Column(IsIdentity = false, IsPrimary = true)]
    [Snowflake]
    public long Id { get; set; }

    /// <inheritdoc />
    [Description("是否删除")]
    [Column]
    public bool IsDeleted { get; set; }

}
