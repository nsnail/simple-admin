using DataType = FreeSql.DataType;

namespace SimpleAdmin.WebApi.Infrastructure.Configuration.Options;

/// <summary>
///     数据库连接字符串配置
/// </summary>
public record DatabaseOptions : OptionAbstraction
{
    /// <summary>
    ///     链接字符串
    /// </summary>
    public string ConnStr { get; set; }

    /// <summary>
    ///     建库脚本路径、为空不自动建库
    /// </summary>
    public string DbCreationFile { get; set; }

    /// <summary>
    ///     数据库类型
    /// </summary>
    public DataType DbType { get; set; }

    /// <summary>
    ///     启用多租户
    /// </summary>
    public bool IsMultiTenant { get; set; }

    /// <summary>
    ///     是否同步数据结构
    /// </summary>
    public bool IsSyncStructure { get; set; }
}