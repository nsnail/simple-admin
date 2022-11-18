using System.Linq.Expressions;
using FreeSql;
using FreeSql.Internal.Model;
using SimpleAdmin.WebApi.DataContracts;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

namespace SimpleAdmin.WebApi.Repositories;

/// <inheritdoc cref="IRepository{TTable}" />
public class Repository<TTable> : DefaultRepository<TTable, long>, IRepository<TTable>
    where TTable : class, ITable, new()
{
    /// <inheritdoc />
    public Repository(IFreeSql fsql) : base(fsql)
    { }

    /// <inheritdoc />
    public Repository(IFreeSql fsql, Expression<Func<TTable, bool>> filter) : base(fsql, filter)
    { }

    /// <inheritdoc />
    protected Repository(IFreeSql fsql, UnitOfWorkManager uowManger) : base(fsql, uowManger)
    { }


    /// <inheritdoc />
    public virtual async Task<bool> DeleteRecursiveAsync(Expression<Func<TTable, bool>> exp,
                                                         params string[]                disableGlobalFilterNames)
    {
        await Select.Where(exp)
                    .DisableGlobalFilter(disableGlobalFilterNames)
                    .AsTreeCte()
                    .ToDelete()
                    .ExecuteAffrowsAsync();

        return true;
    }

    /// <inheritdoc />
    public virtual Task<TDto> GetAsync<TDto>(long id)
    {
        return Select.WhereDynamic(id).ToOneAsync<TDto>();
    }

    /// <inheritdoc />
    public virtual Task<TDto> GetAsync<TDto>(Expression<Func<TTable, bool>> exp)
    {
        return Select.Where(exp).ToOneAsync<TDto>();
    }

    /// <inheritdoc />
    public virtual Task<TTable> GetAsync(Expression<Func<TTable, bool>> exp)
    {
        return Select.Where(exp).ToOneAsync();
    }

    public virtual async Task<(List<TTable> list, long total)> GetPagedListAsync(
        DynamicFilterInfo dynamicFilterInfo,
        int               page,
        int               pageSize)
    {
        var list = await Select.WhereDynamicFilter(dynamicFilterInfo)
                               .Count(out var total)
                               .Page(page, pageSize)
                               .ToListAsync();
        return (list, total);
    }

    /// <inheritdoc />
    public virtual async Task<bool> SoftDeleteAsync(long id)
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
    public virtual async Task<bool> SoftDeleteAsync(long[] ids)
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
    public virtual async Task<bool> SoftDeleteAsync(Expression<Func<TTable, bool>> exp,
                                                    params string[]                disableGlobalFilterNames)
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
    public virtual async Task<bool> SoftDeleteRecursiveAsync(Expression<Func<TTable, bool>> exp,
                                                             params string[]                disableGlobalFilterNames)
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
    public ContextUser ContextUser { get; set; }
}