using System;
using System.Threading;
using System.Threading.Tasks;
using FreeSql;
using MediatR;
using Microsoft.Extensions.Logging;
using NSExt.Extensions;

namespace SimpleAdmin.Rest.Core.Pipelines;

/// <summary>
///     freeSql事务管道
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class FreeSqlTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<FreeSqlTransactionBehavior<TRequest, TResponse>> _logger;
    private          IUnitOfWork                                              _unitOfWork;
    private readonly UnitOfWorkManager                                        _unitOfWorkManager;

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="unitOfWorkManager">工作单元管理管理器</param>
    /// <param name="logger"></param>
    public FreeSqlTransactionBehavior(UnitOfWorkManager                                        unitOfWorkManager,
                                      ILogger<FreeSqlTransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _logger            = logger;
    }


    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest                          request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken                 cancellationToken)
    {
        TResponse response;
        if (TryBegin(request)) {
            int? hashCode = _unitOfWork.GetHashCode();
            try {
                response = await next();
                _logger.Info($"----- 拦截同步执行的方法-事务 {hashCode} 提交前----- ");
                _unitOfWork.Commit();
                _logger.Info($"----- 拦截同步执行的方法-事务 {hashCode} 提交成功----- ");
            }
            catch (Exception) {
                _logger.Error($"----- 拦截同步执行的方法-事务 {hashCode} 提交失败----- ");
                _unitOfWork.Rollback();
                throw;
            }
            finally {
                _unitOfWork.Dispose();
            }
        }
        else {
            response = await next();
        }


        return response;
    }

    private bool TryBegin(TRequest request)
    {
        if (request.GetType().GetInterface("ICommand") is null) return false;
        _unitOfWork = _unitOfWorkManager.Begin();
        return true;
    }
}