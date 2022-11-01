﻿using Furion.DynamicApiController;
using MediatR;

// ReSharper disable ContextualLoggerProblem

namespace SimpleAdmin.Rest.Main.Api;

/// <summary>
///     控制器基类
/// </summary>
/// <typeparam name="TLogType">日志类型</typeparam>
public abstract class ApiBase<TLogType> : IDynamicApiController
{
    /// <summary>
    ///     日志记录器
    /// </summary>
    public ILogger<TLogType> Logger { get; }

    /// <summary>
    ///     CQRS Mediator
    /// </summary>
    protected IMediator Mediator { get; }

    /// <summary>
    ///     控制器基类
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    protected ApiBase(ILogger<TLogType> logger, IMediator mediator) : this(logger)
    {
        Mediator = mediator;
    }

    /// <summary>
    ///     控制器基类
    /// </summary>
    /// <param name="logger"></param>
    protected ApiBase(ILogger<TLogType> logger)
    {
        Logger = logger;
    }
}