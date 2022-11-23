// ReSharper disable ContextualLoggerProblem

using Furion.DynamicApiController;
using SimpleAdmin.WebApi.DataContracts;
using SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;
using SimpleAdmin.WebApi.Repositories;

namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     Api Controller 基类
/// </summary>
public abstract class ApiBase<TLogger, TTable> : ApiBase<TLogger> where TTable : DataContract, ITable, new()
{
    /// <summary>
    ///     Api Controller 基类
    /// </summary>
    protected ApiBase()
    {
        Repository = App.GetService<Repository<TTable>>();
    }

    /// <param name="logger"></param>
    /// <param name="repository"></param>
    protected ApiBase(ILogger<TLogger> logger, Repository<TTable> repository) : base(logger)
    {
        Repository = repository;
    }


    /// <summary>
    ///     默认仓储
    /// </summary>
    protected Repository<TTable> Repository { get; }
}

/// <summary>
///     Api Controller 基类
/// </summary>
public abstract class ApiBase<T> : IDynamicApiController
{
    /// <summary>
    ///     Api Controller 基类
    /// </summary>
    protected ApiBase()
    {
        Logger = App.GetService<ILogger<T>>();
    }

    /// <param name="logger"></param>
    protected ApiBase(ILogger<T> logger)
    {
        Logger = logger;
    }

    /// <summary>
    ///     日志记录器
    /// </summary>
    protected ILogger<T> Logger { get; }
}