namespace SimpleAdmin.Infrastructure.Configuration.Options.Nodes.Connection;

/// <summary>
///     数据库服务器节点
/// </summary>
public record ServersNode
{
    /// <summary>
    ///     是否自动同步数据结构
    /// </summary>
    public bool AutoSyncStructure { get; set; }

    /// <summary>
    ///     链接字符串
    /// </summary>
    public string ConnStr { get; set; }

    /// <summary>
    ///     数据库类型
    /// </summary>
    public string DbType { get; set; }

    /// <summary>
    ///     连接名称
    /// </summary>
    public string Name { get; set; }
}



