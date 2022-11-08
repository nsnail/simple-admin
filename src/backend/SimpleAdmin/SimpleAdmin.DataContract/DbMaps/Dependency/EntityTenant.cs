using System.ComponentModel;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;

namespace SimpleAdmin.DataContract.DbMaps.Dependency;

/// <summary>
///     实体租户
/// </summary>
public record EntityTenant : EntityBase, ITenant
{
    /// <summary>
    ///     租户Id
    /// </summary>
    [Description("租户Id")]
    [Column(Position = 2, CanUpdate = false)]
    [JsonProperty(Order = -20)]
    public long? TenantId { get; set; }
}