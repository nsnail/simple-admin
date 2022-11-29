using System.Linq.Expressions;
using FreeSql;
using SimpleAdmin.Infrastructure.Identity;

namespace SimpleAdmin.Infrastructure.Repositories;

public class RepositoryBase<TEntity, TKey> : DefaultRepository<TEntity, TKey>, IRepositoryBase<TEntity, TKey>
    where TEntity : class
{
    /// <inheritdoc />
    public IContextUser ContextUser { get; set; }


    /// <inheritdoc />
    public virtual async Task<bool> DeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp,
                                                         params string[]                 disableGlobalFilterNames)
    {
        await Select.Where(exp)
                    .DisableGlobalFilter(disableGlobalFilterNames)
                    .AsTreeCte()
                    .ToDelete()
                    .ExecuteAffrowsAsync();

        return true;
    }

    /// <inheritdoc />
    public virtual Task<TDto> GetAsync<TDto>(TKey id)
    {
        return Select.WhereDynamic(id).ToOneAsync<TDto>();
    }

    /// <inheritdoc />
    public virtual Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp)
    {
        return Select.Where(exp).ToOneAsync<TDto>();
    }

    /// <inheritdoc />
    public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp)
    {
        return Select.Where(exp).ToOneAsync();
    }

    /// <inheritdoc />
    public virtual async Task<bool> SoftDeleteAsync(TKey id)
    {
        await UpdateDiy.SetDto(new {
                            IsDeleted        = true,
                            ModifiedUserId   = ContextUser.Id,
                            ModifiedUserName = ContextUser.UserName
                        })
                       .WhereDynamic(id)
                       .ExecuteAffrowsAsync();

        return true;
    }

    /// <inheritdoc />
    public virtual async Task<bool> SoftDeleteAsync(TKey[] ids)
    {
        await UpdateDiy.SetDto(new {
                            IsDeleted        = true,
                            ModifiedUserId   = ContextUser.Id,
                            ModifiedUserName = ContextUser.UserName
                        })
                       .WhereDynamic(ids)
                       .ExecuteAffrowsAsync();

        return true;
    }

    /// <inheritdoc />
    public virtual async Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> exp,
                                                    params string[]                 disableGlobalFilterNames)
    {
        await UpdateDiy.SetDto(new {
                            IsDeleted        = true,
                            ModifiedUserId   = ContextUser.Id,
                            ModifiedUserName = ContextUser.UserName
                        })
                       .Where(exp)
                       .DisableGlobalFilter(disableGlobalFilterNames)
                       .ExecuteAffrowsAsync();

        return true;
    }

    /// <inheritdoc />
    public virtual async Task<bool> SoftDeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp,
                                                             params string[]                 disableGlobalFilterNames)
    {
        await Select.Where(exp)
                    .DisableGlobalFilter(disableGlobalFilterNames)
                    .AsTreeCte()
                    .ToUpdate()
                    .SetDto(new {
                         IsDeleted        = true,
                         ModifiedUserId   = ContextUser.Id,
                         ModifiedUserName = ContextUser.UserName
                     })
                    .ExecuteAffrowsAsync();

        return true;
    }


    /// <inheritdoc />
    public RepositoryBase(IFreeSql fsql) : base(fsql)
    { }

    /// <inheritdoc />
    public RepositoryBase(IFreeSql fsql, Expression<Func<TEntity, bool>> filter) : base(fsql, filter)
    { }

    /// <inheritdoc />
    protected RepositoryBase(IFreeSql fsql, UnitOfWorkManager uowManger) : base(fsql, uowManger)
    { }
}