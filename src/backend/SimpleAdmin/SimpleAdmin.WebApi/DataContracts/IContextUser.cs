namespace SimpleAdmin.WebApi.DataContracts;

/// <summary>
///     上下文用户信息
/// </summary>
public interface IContextUser
{
    /// <summary>
    ///     用户Id
    /// </summary>
    long Id { get; }

    /// <summary>
    ///     用户名
    /// </summary>
    string UserName { get; }
}

/// <inheritdoc cref="SimpleAdmin.WebApi.DataContracts.IContextUser" />
public record ContextUser : IContextUser, IScoped
{
    /// <inheritdoc />
    public long Id { get; }

    /// <inheritdoc />
    public string UserName { get; }
}