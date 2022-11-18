// ReSharper disable ContextualLoggerProblem

using Furion.DynamicApiController;

namespace SimpleAdmin.WebApi.Api;

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