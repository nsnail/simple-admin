using System.ComponentModel;
using System.Text.Json.Serialization;
using FreeSql.DataAnnotations;
using SimpleAdmin.WebApi.Aop.Attributes;
using SimpleAdmin.WebApi.Infrastructure.Constant;

namespace SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

/// <summary>
///     全功能表
/// </summary>
public abstract record FullTable : DataContract,
                                   ITable,
                                   IFieldPrimary,
                                   IFieldAdd,
                                   IFieldUpdate,
                                   IFieldDelete,
                                   IFieldVersion
{
    /// <inheritdoc />
    [JsonIgnore]
    [Description(Strings.DSC_CREATED_TIME)]
    [Column(CanUpdate = false, ServerTime = DateTimeKind.Local)]
    public DateTime CreatedTime { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    [Description(Strings.DSC_CREATED_USER_ID)]
    [Column(CanUpdate = false)]
    public long? CreatedUserId { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    [Description(Strings.DSC_CREATED_USER_NAME)]
    [Column(CanUpdate = false)]
    public string CreatedUserName { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    [Description(Strings.DSC_ID)]
    [Column(IsIdentity = false, IsPrimary = true)]
    [Snowflake]
    public long Id { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    [Description(Strings.DSC_IS_DELETED)]
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    [Description(Strings.DESC_MODIFIED_TIME)]
    [Column(CanInsert = false, ServerTime = DateTimeKind.Local)]
    public DateTime? ModifiedTime { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    [Description(Strings.DSC_MODIFIED_USER_ID)]
    [Column(CanInsert = false)]
    public long? ModifiedUserId { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    [Description(Strings.DSC_MODIFIED_USER_NAME)]
    [Column(CanInsert = false)]
    public string ModifiedUserName { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    [Description(Strings.DSC_VERSION)]
    [Column(IsVersion = true)]
    public long Version { get; set; }
}