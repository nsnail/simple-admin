using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;
using SimpleAdmin.WebApi.Infrastructure.Constant;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps;

/// <summary>
///     权限（菜单）表
/// </summary>
[Table]
public record TbSysPermission : FullTable, IFieldBitSet
{
    /// <inheritdoc />
    public virtual long BitSet { get; set; }

    /// <summary>
    ///     子节点
    /// </summary>
    [Navigate(nameof(ParentId))]
    public virtual List<TbSysPermission> Children { get; set; }


    /// <summary>
    ///     权限编码
    /// </summary>
    public virtual string Code { get; set; }

    /// <summary>
    ///     组件
    /// </summary>
    public virtual string Component { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    public virtual string Description { get; set; }

    /// <summary>
    ///     图标
    /// </summary>

    public virtual string Icon { get; set; }

    /// <summary>
    ///     权限名称
    /// </summary>
    public virtual string Label { get; set; }


    /// <summary>
    ///     父级节点
    /// </summary>
    public virtual long ParentId { get; set; }

    /// <summary>
    ///     菜单访问地址
    /// </summary>
    public virtual string Path { get; set; }

    /// <summary>
    ///     排序
    /// </summary>
    public virtual int? Sort { get; set; } = 0;


    /// <summary>
    ///     权限类型
    /// </summary>
    [Column(CanUpdate = false)]
    public virtual Enums.PermissionTypes Type { get; set; }
}