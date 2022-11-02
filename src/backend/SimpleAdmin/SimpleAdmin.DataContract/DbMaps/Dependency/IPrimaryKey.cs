using System.ComponentModel;
using FreeSql.DataAnnotations;
using SimpleAdmin.Infrastructure.Attributes;

namespace SimpleAdmin.DataContract.DbMaps.Dependency;

/// <summary>
///     主键接口
/// </summary>
public interface IPrimaryKey<TKey>
{
    /// <summary>
    ///     主键Id
    /// </summary>
    [Description("主键Id")]
    [Snowflake]
    [Column(Position = 1, IsIdentity = false, IsPrimary = true)]
    TKey Id { get; set; }
}