﻿using Furion.DataValidation;
using Furion.FriendlyException;
using Furion.UnifyResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleAdmin.WebApi.DataContracts.Dto;
using SimpleAdmin.WebApi.Infrastructure.Constants;

namespace SimpleAdmin.WebApi.Aop.Filters;

/// <summary>
///     Api结果格式化处理器
///     协议：
///     当接口成功执行： errorCode = 0， http statusCode = 2xx，
///     当接口异常：
///     a） 参数不合法：
///     errorCode = 401x， http statusCode = 400，
///     b） 操作不合法（业务异常）：
///     errorCode = 402x， http statusCode = 400，
///     c） 权限不合法：
///     errorCode = 403x， http statusCode = 401，403
///     d） 安全保护：
///     errorCode = 404x， http statusCode = 400
///     e）未处理的异常：
///     errorCode = 4000， http statusCode = 400
///     f) webserver级错误：
///     底层错误，未进入应用程序管道 （无封装），无errorCode， http statusCode = 404 （not found） ，405 （not allowed） ，502 （bad gateway）等等，
///     一般不会与本程序处理过的异常重合。
/// </summary>
[SuppressSniffer]
[UnifyModel(typeof(RestfulInfo<>))]
public class ApiResultHandler : IUnifyResultProvider
{
    /// <summary>
    ///     业务异常
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
    {
        var result = metadata.OriginErrorCode is null
                         ? RestfulResult(Enums.ErrorCodes.未知错误)
                         : RestfulResult(metadata.OriginErrorCode, metadata.Data, metadata.Errors);

        return new JsonResult(result) {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }

    /// <inheritdoc />
    public Task OnResponseStatusCodes(HttpContext                context,
                                      int                        statusCode,
                                      UnifyResultSettingsOptions unifyResultSettings = default)
    {
        throw new NotImplementedException();
    }

    // /// <summary>
    // ///     特定状态码返回值
    // /// </summary>
    // /// <param name="context"></param>
    // /// <param name="statusCode"></param>
    // /// <param name="unifyResultSettings"></param>
    // /// <returns></returns>
    // public async Task OnResponseStatusCodes(HttpContext                context,
    //                                         int                        statusCode,
    //                                         UnifyResultSettingsOptions unifyResultSettings)
    // {
    //     // 设置响应状态码
    //     UnifyContext.SetResponseStatusCodes(context, statusCode, unifyResultSettings);
    //
    //     var jsonOptions = App.GetOptions<JsonOptions>();
    //     switch (statusCode) {
    //         // 处理 401 状态码
    //         case StatusCodes.Status401Unauthorized:
    //             await context.Response.WriteAsJsonAsync(RestfulResult(ErrorCodes.未登录, null, nameof(ErrorCodes.未登录)),
    //                                                     jsonOptions?.JsonSerializerOptions);
    //             break;
    //         // 处理 403 状态码
    //         case StatusCodes.Status403Forbidden:
    //             await context.Response.WriteAsJsonAsync(RestfulResult(ErrorCodes.未授权, null, nameof(ErrorCodes.未授权)),
    //                                                     jsonOptions?.JsonSerializerOptions);
    //             break;
    //     }
    // }

    /// <summary>
    ///     成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public IActionResult OnSucceeded(ActionExecutedContext context, object data)
    {
        return new JsonResult(RestfulResult(0, data));
    }

    /// <summary>
    ///     验证失败
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnValidateFailed(ActionExecutingContext context, ValidationMetadata metadata)
    {
        return new JsonResult(RestfulResult(Enums.ErrorCodes.无效输入, metadata.Data, metadata.ValidationResult)) {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }

    /// <summary>
    ///     返回 RESTful 风格结果集
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    private static RestfulInfo<dynamic> RestfulResult(object errorCode, object data = default, object message = default)
    {
        return new RestfulInfo<dynamic> {
            Code    = errorCode,
            Data    = data,
            Message = message
        };
    }
}