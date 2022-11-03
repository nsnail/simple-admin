using SimpleAdmin.Infrastructure.Configuration.Options.Nodes.Connection;

namespace SimpleAdmin.Infrastructure.Configuration.Options;

/// <summary>
///     数据库连接字符串配置
/// </summary>
public record ConnectionsOptions : OptionAbstraction
{
    /// <summary>
    ///     服务器节点列表
    /// </summary>
    public List<ServersNode> Servers { get; set; }

    /// <summary>
    ///     定位服务器节点
    /// </summary>
    /// <param name="name">连接名称</param>
    public ServersNode this[string name] {
        get { return Servers.Single(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); }
    }
}