namespace SimpleAdmin.DataContract.DbMaps.Dependency;

/// <summary>
///     实体基类
/// </summary>
public abstract record EntityBase<TKey> : IPrimaryKey<TKey>, IEntityAdd, IEntityUpdate, IEntityDelete, IVersion
    where TKey : struct
{
    /// <inheritdoc />
    public DateTime? CreatedTime { get; set; }

    /// <inheritdoc />
    public long? CreatedUserId { get; set; }

    /// <inheritdoc />
    public string CreatedUserName { get; set; }

    /// <inheritdoc />
    public TKey Id { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public DateTime? ModifiedTime { get; set; }

    /// <inheritdoc />
    public long? ModifiedUserId { get; set; }

    /// <inheritdoc />
    public string ModifiedUserName { get; set; }

    /// <inheritdoc />
    public long Version { get; set; }
}

/// <summary>
///     实体基类
/// </summary>
public abstract record EntityBase : EntityBase<long>
{ }