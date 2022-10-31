using System;
using System.Threading.Tasks;
using Furion;
using Furion.DataValidation;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using SimpleAdmin.Core.Constant;

namespace SimpleAdmin.Web.Core.Pipeline;

/// <summary>
///     Api响应结果格式化器
/// </summary>
[SuppressSniffer]
[UnifyModel(typeof(RestfulResultTmpl<>))]
public class ApiResultHandler : IUnifyResultProvider
{
    private static JsonResult CreateJsonResult(ErrorCodes errorCode,
                                               bool       success,
                                               dynamic    data   = null,
                                               dynamic    errors = null)
    {
        return new JsonResult(new RestfulResultTmpl<dynamic> {
            ErrorCode = errorCode,
            Succeeded = success,
            Data      = data,
            Errors    = errors,
            Extras    = UnifyContext.Take(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        }) {
            StatusCode = StatusCodes.Status200OK
        };
    }


    /// <inheritdoc />
    public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
    {
        // 解析异常信息
        var statusCode = metadata.ErrorCode is null ? ErrorCodes.未知错误 : (ErrorCodes)metadata.OriginErrorCode;
        //errorCode ==null，是未捕获的.net exception，在正式环境下需要屏蔽异常信息信息。
        var errors                                                                        = metadata.Errors;
        if (metadata.ErrorCode is null && !App.WebHostEnvironment.IsDevelopment()) errors = Const.Messages.好像出了点问题请稍后再试;
        return CreateJsonResult(statusCode, false, null, errors);
    }


    /// <inheritdoc />
    public Task OnResponseStatusCodes(HttpContext                context,
                                      int                        statusCode,
                                      UnifyResultSettingsOptions unifyResultSettings = null)
    {
        throw new NotImplementedException();
    }

    ///// <summary>
    ///// </summary>
    ///// <param name="context"></param>
    ///// <param name="statusCode"></param>
    ///// <returns></returns>
    //public async Task OnResponseStatusCodes(HttpContext context, int statusCode)
    //{
    //    //switch (statusCode) {
    //    //	// 处理 401 状态码
    //    //	case StatusCodes.Status401Unauthorized:
    //    //		await context.Response.WriteAsJsonAsync(new RESTfulResult<object> {
    //    //			StatusCode = StatusCodes.Status401Unauthorized,
    //    //			Succeeded = false,
    //    //			Data = null,
    //    //			Errors = "401 Unauthorized",
    //    //			Extras = UnifyContext.Take(),
    //    //			Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
    //    //		});
    //    //		break;
    //    //	// 处理 403 状态码
    //    //	case StatusCodes.Status403Forbidden:
    //    //		await context.Response.WriteAsJsonAsync(new RESTfulResult<object> {
    //    //			StatusCode = StatusCodes.Status403Forbidden,
    //    //			Succeeded = false,
    //    //			Data = null,
    //    //			Errors = "403 Forbidden",
    //    //			Extras = UnifyContext.Take(),
    //    //			Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
    //    //		});
    //    //		break;

    //    //	default:
    //    //		break;
    //    //}
    //}


    /// <inheritdoc />
    // ReSharper disable once RedundantAssignment
    public IActionResult OnSucceeded(ActionExecutedContext context, object data)
    {
        switch (context.Result) {
            case JsonResult jsonResult:
                data = jsonResult.Value;
                return CreateJsonResult(0, true, data);
        }

        return context.Result;
    }

    /// <inheritdoc />
    public IActionResult OnValidateFailed(ActionExecutingContext context, ValidationMetadata metadata)
    {
        return CreateJsonResult(ErrorCodes.无效输入, false, null, metadata.ValidationResult);
    }
}




