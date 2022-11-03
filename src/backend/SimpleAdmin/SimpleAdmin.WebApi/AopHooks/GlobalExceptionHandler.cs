using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;
using NSExt.Extensions;
using Serilog;

namespace SimpleAdmin.WebApi.AopHooks;

/// <summary>
///     全局异常处理
/// </summary>
public class GlobalExceptionHandler : IGlobalExceptionHandler, ISingleton
{
    /// <inheritdoc />
    public Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is not AppFriendlyException)
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            Log.Error(context.Exception + Environment.NewLine + new {
                          context.HttpContext.Request.Path,
                          Query   = context.HttpContext.Request.QueryString,
                          Headers = context.HttpContext.Request.Headers.Json()
                      }.Json(),
                      string.Empty);
        return Task.CompletedTask;
    }
}