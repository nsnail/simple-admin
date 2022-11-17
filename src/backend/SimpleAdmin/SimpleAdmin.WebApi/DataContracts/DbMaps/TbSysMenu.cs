using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps;

/// <summary>
///     树形菜单表
/// </summary>
[Table]
public record TbSysMenu : DefaultTable
{
    /// <summary>
    ///     子节点
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<TbSysMenu> Childs { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }


    [Navigate(nameof(ParentId))] public long ParentId { get; set; }
}