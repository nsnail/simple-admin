using SimpleAdmin.Infrastructure.Constant;

namespace SimpleAdmin.Infrastructure.Identity;

/// <summary>
///     上下文用户信息
/// </summary>
public interface IContextUser
{
    /// <summary>
    ///     数据权限
    /// </summary>
    DataPermissionInfo DataPermission { get; }


    /// <summary>
    ///     用户Id
    /// </summary>
    long Id { get; }

    /// <summary>
    ///     用户名
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// 租户id
    /// </summary>
    public long TenantId { get;  }

    /// <summary>
    /// 用户类型
    /// </summary>
    public  Const.Enums.UserTypes Type { get;  }
}

public class ContextUser : IContextUser, IScoped
{
    /// <inheritdoc />
    public DataPermissionInfo    DataPermission { get; }

    /// <inheritdoc />
    public long                  Id             { get; }

    /// <inheritdoc />
    public string                UserName       { get; }

    /// <inheritdoc />
    public long                  TenantId       { get; }

    /// <inheritdoc />
    public Const.Enums.UserTypes Type           { get; }
}