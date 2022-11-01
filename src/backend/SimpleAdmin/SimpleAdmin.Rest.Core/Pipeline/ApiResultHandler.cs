using System.Threading.Tasks;
using Furion;
using Furion.DataValidation;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SimpleAdmin.Rest.Core.Pipeline;

/// <summary>
///     Api结果格式化处理器
/// </summary>
[SuppressSniffer]
[UnifyModel(typeof(RestfulResultTmpl<>))]
public class ApiResultHandler : IUnifyResultProvider
{
    /// <summary>
    ///     异常返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
    {
        //JsonResult 第二个参数可配置独立的序列化属性
        return new JsonResult(RestfulResult(metadata.StatusCode, metadata.Data, metadata.Errors.ToString()));
    }

    /// <summary>
    ///     成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public IActionResult OnSucceeded(ActionExecutedContext context, object data)
    {
        // JsonResult 第二个参数可配置独立的序列化属性
        return new JsonResult(RestfulResult(StatusCodes.Status200OK, data));
    }

    /// <summary>
    ///     验证失败/业务异常返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnValidateFailed(ActionExecutingContext context, ValidationMetadata metadata)
    {
        // JsonResult 第二个参数可配置独立的序列化属性
        return new JsonResult(RestfulResult(metadata.StatusCode ?? StatusCodes.Status400BadRequest,
                                            metadata.Data,
                                            metadata.ValidationResult.ToString()));
    }

    /// <summary>
    ///     特定状态码返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="statusCode"></param>
    /// <param name="unifyResultSettings"></param>
    /// <returns></returns>
    public async Task OnResponseStatusCodes(HttpContext                context,
                                            int                        statusCode,
                                            UnifyResultSettingsOptions unifyResultSettings)
    {
        // 设置响应状态码
        UnifyContext.SetResponseStatusCodes(context, statusCode, unifyResultSettings);

        switch (statusCode) {
            // 处理 401 状态码
            case StatusCodes.Status401Unauthorized:
                await context.Response.WriteAsJsonAsync(RestfulResult(statusCode, "401 Unauthorized"),
                                                        App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                break;
            // 处理 403 状态码
            case StatusCodes.Status403Forbidden:
                await context.Response.WriteAsJsonAsync(RestfulResult(statusCode, "403 Forbidden"),
                                                        App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                break;
        }
    }

    /// <summary>
    ///     返回 RESTful 风格结果集
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    private static RestfulResultTmpl<dynamic> RestfulResult(int    statusCode,
                                                            object data    = default,
                                                            string message = default)
    {
        return new RestfulResultTmpl<dynamic> {
            Code    = statusCode,
            Data    = data,
            Message = message
        };
    }
}