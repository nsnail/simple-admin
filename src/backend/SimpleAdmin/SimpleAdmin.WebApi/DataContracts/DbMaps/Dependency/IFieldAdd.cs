namespace SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

/// <summary>
///     新增字段接口
/// </summary>
public interface IFieldAdd
{
    /// <summary>
    ///     创建时间
    /// </summary>
    DateTime CreatedTime { get; set; }

    /// <summary>
    ///     创建者用户Id
    /// </summary>
    long? CreatedUserId { get; set; }

    /// <summary>
    ///     创建者
    /// </summary>
    string CreatedUserName { get; set; }
}