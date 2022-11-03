using SimpleAdmin.Infrastructure.Constant;

namespace SimpleAdmin.Infrastructure.Identity;

public record DataPermissionInfo
{
    /// <summary>
    ///     数据范围
    /// </summary>
    public Const.Enums.DataScopes DataScopes { get; init; } = Const.Enums.DataScopes.Self;

    /// <summary>
    ///     部门Id
    /// </summary>
    public long OrgId { get; init; }

    /// <summary>
    ///     部门列表
    /// </summary>
    public List<long> OrgIds { get; init; }
}