using System.Linq.Expressions;
using FreeSql;
using SimpleAdmin.WebApi.DataContracts;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

namespace SimpleAdmin.WebApi.Repositories;

/// <summary>
///     基础仓储接口
/// </summary>
/// <typeparam name="TTable"></typeparam>
public interface IRepositoryBase<TTable> : IBaseRepository<TTable> where TTable : class
{
    /// <summary>
    ///     当前上下文关联的用户
    /// </summary>
    IContextUser ContextUser { get; set; }

    /// <summary>
    ///     递归删除
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="disableGlobalFilterNames">禁用全局过滤器名</param>
    /// <returns></returns>
    Task<bool> DeleteRecursiveAsync(Expression<Func<TTable, bool>> exp, params string[] disableGlobalFilterNames);

    /// <summary>
    ///     获得Dto
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<TDto> GetAsync<TDto>(long id);

    /// <summary>
    ///     根据条件获取Dto
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    Task<TDto> GetAsync<TDto>(Expression<Func<TTable, bool>> exp);

    /// <summary>
    ///     根据条件获取实体
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    Task<TTable> GetAsync(Expression<Func<TTable, bool>> exp);

    /// <summary>
    ///     软删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(long id);

    /// <summary>
    ///     批量软删除
    /// </summary>
    /// <param name="ids">主键数组</param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(long[] ids);

    /// <summary>
    ///     软删除
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="disableGlobalFilterNames">禁用全局过滤器名</param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(Expression<Func<TTable, bool>> exp, params string[] disableGlobalFilterNames);

    /// <summary>
    ///     递归软删除
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="disableGlobalFilterNames">禁用全局过滤器名</param>
    /// <returns></returns>
    Task<bool> SoftDeleteRecursiveAsync(Expression<Func<TTable, bool>> exp, params string[] disableGlobalFilterNames);
}

/// <inheritdoc cref="SimpleAdmin.WebApi.Repositories.IRepositoryBase{TTable}" />
public abstract class RepositoryBase<TTable> : DefaultRepository<TTable, long>, IRepositoryBase<TTable>
    where TTable : class, ITable, new()
{
    /// <inheritdoc />
    public IContextUser ContextUser { get; set; }


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
    public RepositoryBase(IFreeSql fsql) : base(fsql)
    { }

    /// <inheritdoc />
    public RepositoryBase(IFreeSql fsql, Expression<Func<TTable, bool>> filter) : base(fsql, filter)
    { }

    /// <inheritdoc />
    protected RepositoryBase(IFreeSql fsql, UnitOfWorkManager uowManger) : base(fsql, uowManger)
    { }
}