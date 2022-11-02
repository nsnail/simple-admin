using System.ComponentModel;
using FreeSql.DataAnnotations;

namespace SimpleAdmin.DataContract.DbMaps.Dependency;

/// <summary>
///     版本接口
/// </summary>
public interface IVersion
{
    /// <summary>
    ///     数据版本
    /// </summary>
    [Description("版本")]
    [Column(Position = -30, IsVersion = true)]
    long Version { get; set; }
}