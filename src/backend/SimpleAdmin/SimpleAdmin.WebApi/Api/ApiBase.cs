using Furion.DynamicApiController;

// ReSharper disable ContextualLoggerProblem

namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     Api Controller 基类
/// </summary>
/// <typeparam name="TLogType">日志类型</typeparam>
public abstract class ApiBase<TLogType> : IDynamicApiController
{
    /// <summary>
    ///     日志记录器
    /// </summary>
    public ILogger<TLogType> Logger { get; }

    /// <summary>
    ///     控制器基类
    /// </summary>
    /// <param name="logger"></param>
    protected ApiBase(ILogger<TLogType> logger)
    {
        Logger = logger;
    }
}