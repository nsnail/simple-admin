using FreeSql;

namespace SimpleAdmin.Infrastructure.Configuration.Options.Nodes.Connection;

/// <summary>
///     数据库服务器节点
/// </summary>
public record ServersNode
{
    /// <summary>
    ///     链接字符串
    /// </summary>
    public string ConnStr { get; set; }

    /// <summary>
    ///     建库脚本路径、为空不自动建库
    /// </summary>
    public string CreateDbFilePath { get; set; }

    /// <summary>
    ///     数据库类型
    /// </summary>
    public DataType DbType { get; set; }

    /// <summary>
    ///     启用多租户
    /// </summary>
    public bool IsMultiTenant { get; set; }

    /// <summary>
    ///     是否同步数据
    /// </summary>
    public bool IsSyncData { get; set; }

    /// <summary>
    ///     是否同步数据结构
    /// </summary>
    public bool IsSyncStructure { get; set; }

    /// <summary>
    ///     连接名称
    /// </summary>
    public string Name { get; set; }
}