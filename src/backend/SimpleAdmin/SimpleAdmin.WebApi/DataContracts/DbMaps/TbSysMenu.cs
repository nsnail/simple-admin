using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;
using SimpleAdmin.WebApi.Infrastructure.Constant;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps;

/// <summary>
///     菜单表
/// </summary>
[Table]
public record TbSysMenu : FullTable, IFieldBitSet
{
    /// <inheritdoc />
    public virtual long BitSet { get; set; }

    /// <summary>
    ///     子节点
    /// </summary>
    [Navigate(nameof(ParentId))]
    public virtual List<TbSysMenu> Children { get; set; }


    /// <summary>
    ///     菜单编码
    /// </summary>
    public virtual string Code { get; set; }

    /// <summary>
    ///     组件
    /// </summary>
    public virtual string Component { get; set; }


    /// <summary>
    ///     图标
    /// </summary>
    public virtual string Icon { get; set; }


    /// <summary>
    ///     父级节点
    /// </summary>
    public virtual long ParentId { get; set; }

    /// <summary>
    ///     菜单路径
    /// </summary>
    public virtual string Path { get; set; }

    /// <summary>
    ///     排序
    /// </summary>
    public virtual int Sort { get; set; }

    /// <summary>
    ///     菜单标题
    /// </summary>
    public virtual string Title { get; set; }


    /// <summary>
    ///     菜单类型
    /// </summary>
    [Column(CanUpdate = false)]
    public virtual Enums.MenuTypes Type { get; set; }
}