using Furion.EventBus;
using Mapster;
using SimpleAdmin.WebApi.Aop.Filters;
using SimpleAdmin.WebApi.DataContracts.DbMaps;
using SimpleAdmin.WebApi.Repositories;

namespace SimpleAdmin.WebApi.Events;

/// <summary>
///     请求审计事件
/// </summary>
public class RequestAuditEvents : IEventSubscriber, ISingleton, IDisposable
{
    private readonly IServiceScope _scope;

    /// <param name="serviceProvider"></param>
    public RequestAuditEvents(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateScope();
    }

    /// <summary>
    ///     保存到数据库
    /// </summary>
    /// <param name="context"></param>
    [EventSubscribe($"{nameof(RequestAuditHandler)}.{nameof(RequestAuditHandler.OnActionExecutionAsync)}")]
    public async Task SaveToDb(EventHandlerExecutingContext context)
    {
        var tbSysOperationLog = context.Source.Payload.Adapt<TbSysOperationLog>();
        await _scope.ServiceProvider.GetRequiredService<OperationLogRepository>().InsertAsync(tbSysOperationLog);
    }


    /// <inheritdoc />
    public void Dispose()
    {
        _scope?.Dispose();
    }
}