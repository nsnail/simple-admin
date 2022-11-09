using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NSExt.Extensions;

namespace SimpleAdmin.WebApi.Aop.Filters;


/// <summary>
///     请求审计日志
/// </summary>
[SuppressSniffer]
public class RequestAuditHandler : IAsyncActionFilter
{
    // ReSharper disable once NotAccessedField.Local
    private readonly ILogger<RequestAuditHandler> _logger;

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="logger"></param>
    public RequestAuditHandler(ILogger<RequestAuditHandler> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        _logger.Info($"##控制器名称## {actionDescriptor?.ControllerTypeInfo.Name}");

        var method = actionDescriptor?.MethodInfo;
        _logger.Info($"##操作名称## {method?.Name}");

        var httpContext = context.HttpContext;
        var httpRequest = httpContext.Request;
        var httpMethod  = httpContext.Request.Method;
        _logger.Info($"##请求方式## {httpMethod}");

        var requestUrl = httpRequest.GetRequestUrlAddress().UrlDe();
        _logger.Info($"##请求地址## {requestUrl}");

        var refererUrl = httpRequest.GetRefererUrlAddress();
        _logger.Info($"##来源地址## {refererUrl}");

        var userAgent = httpRequest.Headers["User-Agent"];
        _logger.Info($"##浏览器标识## {userAgent}");

        var remoteIPv4 = httpContext.GetRemoteIpAddressToIPv4();
        _logger.Info($"##客户端 IP 地址## {remoteIPv4}");

        var localIPv4 = httpContext.GetLocalIpAddressToIPv4();
        _logger.Info($"##服务端 IP 地址## {localIPv4}");

        var environment = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().EnvironmentName;
        _logger.Info($"##服务端运行环境## {environment}");


        var timeOperation = Stopwatch.StartNew();
        var resultContext = await next();
        //============== 这里是执行方法之后获取数据 ====================
        timeOperation.Stop();
        _logger.Info($"##执行耗时## {timeOperation.ElapsedMilliseconds}ms");

        _logger.Info($"状态码：{httpContext.Response.StatusCode}");


        //============== 这里是执行方法之前获取数据 ====================
        // 获取控制器、路由信息
        // var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        // // 获取请求的方法
        // var method = actionDescriptor.MethodInfo;
        // // 获取 HttpContext 和 HttpRequest 对象
        // var httpContext = context.HttpContext;
        // var httpRequest = httpContext.Request;
        // // 获取客户端 Ipv4 地址
        // var remoteIPv4 = httpContext.GetRemoteIpAddressToIPv4();
        // // 获取请求的 Url 地址
        // var requestUrl = httpRequest.GetRequestUrlAddress();
        // // 获取来源 Url 地址
        // var refererUrl = httpRequest.GetRefererUrlAddress();
        // // 获取请求参数（写入日志，需序列化成字符串后存储）
        // var parameters = context.ActionArguments;
        // // 获取操作人（必须授权访问才有值）"userId" 为你存储的 claims type，jwt 授权对应的是 payload 中存储的键名
        // var userId = httpContext.User?.FindFirstValue("userId");
        // // 请求时间
        // var requestedTime = DateTimeOffset.Now;
        // //============== 这里是执行方法之后获取数据 ====================
        // var actionContext = await next();
        // // 获取返回的结果
        // var returnResult = actionContext.Result;
        // // 判断是否请求成功，没有异常就是请求成功
        // var isRequestSucceed = actionContext.Exception == null;
        // // 获取调用堆栈信息，提供更加简单明了的调用和异常堆栈
        // var stackTrace = EnhancedStackTrace.Current();
        // 这里写入日志，或存储到数据库中！！！~~~~~~~~~~~~~~~~~~~~
        // _logger.Debug(returnResult);
    }
}