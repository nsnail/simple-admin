using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Furion.EventBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NSExt.Extensions;

namespace SimpleAdmin.WebApi.Aop.Filters;

/// <summary>
///     请求审计日志
/// </summary>
[SuppressSniffer]
public class RequestAuditHandler : IAsyncActionFilter
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="eventPublisher"></param>
    public RequestAuditHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    private readonly IEventPublisher _eventPublisher;

    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var timeOperation = Stopwatch.StartNew();
        var resultContext = await next();
        timeOperation.Stop();
        var duration = (uint)timeOperation.ElapsedMilliseconds;

        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var (retType, retData) = GetReturnData(resultContext);
        // ReSharper disable once UseObjectOrCollectionInitializer
        var auditData = new {
            Action      = actionDescriptor?.ActionName,
            ClientIp    = context.HttpContext.GetRemoteIpAddressToIPv4(),
            Controller  = actionDescriptor?.ControllerName,
            Duration    = duration,
            Environment = App.WebHostEnvironment.EnvironmentName,
            context.HttpContext.Request.Method,
            ReferUrl           = context.HttpContext.Request.GetRefererUrlAddress(),
            RequestContentType = context.HttpContext.Request.ContentType,
            // RequestParameters = GenerateRequestParameterJson(actionDescriptor?.MethodInfo, context.ActionArguments),
            RequestParameters = context.ActionArguments.Json(),
            RequestUrl        = context.HttpContext.Request.GetRequestUrlAddress(),
            // ResponseRawType    = HandleGenericType(actionDescriptor?.MethodInfo.ReturnType),
            ResponseRawType    = actionDescriptor?.MethodInfo.ReturnType.ToString(),
            ResponseStatusCode = (ushort)context.HttpContext.Response.StatusCode,
            // ResponseWrapType   = HandleGenericType(retType),
            ResponseWrapType = retType?.ToString(),
            ResponseResult   = retData?.Json(),
            ServerIp         = context.HttpContext.GetLocalIpAddressToIPv4(),
            UserAgent        = context.HttpContext.Request.Headers["User-Agent"]
        };


        // 发布审计事件
        await _eventPublisher.PublishAsync($"{nameof(RequestAuditHandler)}.{nameof(OnActionExecutionAsync)}",
                                           auditData);


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


    /// <summary>
    ///     检查是否是有效的结果（可进行规范化的结果）
    /// </summary>
    /// <param name="result"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private static bool CheckVaildResult(IActionResult result, out object data)
    {
        data = default;

        // 排除以下结果，跳过规范化处理
        var isDataResult = result switch {
                               ViewResult             => false,
                               PartialViewResult      => false,
                               FileResult             => false,
                               ChallengeResult        => false,
                               SignInResult           => false,
                               SignOutResult          => false,
                               RedirectToPageResult   => false,
                               RedirectToRouteResult  => false,
                               RedirectResult         => false,
                               RedirectToActionResult => false,
                               LocalRedirectResult    => false,
                               ForbidResult           => false,
                               ViewComponentResult    => false,
                               PageResult             => false,
                               NotFoundResult         => false,
                               NotFoundObjectResult   => false,
                               _                      => true
                           };

        // 目前支持返回值 ActionResult
        if (isDataResult)
            data = result switch {
                       // 处理内容结果
                       ContentResult content => content.Content,
                       // 处理对象结果
                       ObjectResult obj => obj.Value,
                       // 处理 JSON 对象
                       JsonResult json => json.Value,
                       _               => null
                   };

        return isDataResult;
    }


    /// <summary>
    ///     生成请求参数信息日志模板
    /// </summary>
    /// <param name="method"></param>
    /// <param name="parameterValues"></param>
    /// <returns></returns>
    private static string GenerateRequestParameterJson(MethodBase method, IDictionary<string, object> parameterValues)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        writer.WriteStartObject();
        writer.WritePropertyName("parameters");
        writer.WriteStartArray();
        foreach (var parameter in method?.GetParameters() ?? Array.Empty<ParameterInfo>()) {
            parameterValues.TryGetValue(parameter.Name!, out var value);
            var parameterType = parameter.ParameterType;
            writer.WriteStartObject();
            writer.WriteString("name", parameter.Name);
            writer.WriteString("type", HandleGenericType(parameterType));

            switch (value) {
                // 文件类型参数
                case IFormFile or List<IFormFile>: {
                    writer.WritePropertyName("value");

                    switch (value) {
                        // 单文件
                        case IFormFile formFile: {
                            writer.WriteStartObject();
                            writer.WriteString(parameter.Name, formFile.Name);
                            writer.WriteString("fileName",     formFile.FileName);
                            writer.WriteNumber("length", formFile.Length);
                            writer.WriteString("contentType", formFile.ContentType);
                            writer.WriteEndObject();

                            goto writeEndObject;
                        }
                        // 多文件
                        case List<IFormFile> formFiles: {
                            writer.WriteStartArray();
                            foreach (var file in formFiles) {
                                writer.WriteStartObject();
                                writer.WriteString(parameter.Name, file.Name);
                                writer.WriteString("fileName",     file.FileName);
                                writer.WriteNumber("length", file.Length);
                                writer.WriteString("contentType", file.ContentType);
                                writer.WriteEndObject();
                            }

                            writer.WriteEndArray();

                            goto writeEndObject;
                        }
                    }

                    break;
                }
                // 处理 byte[] 参数类型
                case byte[] byteArray:
                    writer.WritePropertyName("value");


                    writer.WriteStartObject();
                    writer.WriteNumber("length", byteArray.Length);
                    writer.WriteEndObject();

                    goto writeEndObject;
                // 处理基元类型，字符串类型和空值
                default: {
                    if (parameterType.IsPrimitive || value is string or null) {
                        writer.WritePropertyName("value");
                        switch (value) {
                            case null:
                                writer.WriteNullValue();
                                break;
                            case string str:
                                writer.WriteStringValue(str);
                                break;
                            default: {
                                if (double.TryParse(value.ToString(), out var r))
                                    writer.WriteNumberValue(r);
                                else
                                    writer.WriteStringValue(value.ToString());
                                break;
                            }
                        }
                    }
                    // 其他类型统一进行序列化
                    else {
                        writer.WritePropertyName("value");
                        object rawValue = TrySerializeObject(value, out var succeed);

                        if (succeed)
                            writer.WriteRawValue(rawValue?.ToString() ?? string.Empty);
                        else
                            writer.WriteNullValue();
                    }

                    break;
                }
            }

            writeEndObject:
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Flush();

        // 获取 json 字符串
        return Encoding.UTF8.GetString(stream.ToArray());
    }


    private static (Type, object) GetReturnData(ActionExecutedContext resultContext)
    {
        Type type;

        // 解析返回值
        if (CheckVaildResult(resultContext.Result, out var data)) {
            type = data?.GetType();
        }
        // 处理文件类型
        else if (resultContext.Result is FileResult fileResult) {
            data = new {
                FileName = fileResult.FileDownloadName,
                fileResult.ContentType,
                Length = fileResult is FileContentResult fileContentResult
                             ? (object)fileContentResult.FileContents.Length
                             : null
            };

            type = fileResult.GetType();
        }
        else {
            type = resultContext.Result?.GetType();
        }

        return (type, data);
    }


    /// <summary>
    ///     处理泛型类型转字符串打印问题
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static string HandleGenericType(Type type)
    {
        if (type == null) return string.Empty;
        // 处理泛型类型问题
        if (!type.IsConstructedGenericType) return type.FullName;
        var prefix = type.GetGenericArguments()
                         .Select(HandleGenericType)
                         .Aggregate((previous, current) => previous + current);

        // 通过 _ 拼接多个泛型
        return type.FullName!.Split('`').First() + "_" + prefix;
    }


    /// <summary>
    ///     序列化对象
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="succeed"></param>
    /// <returns></returns>
    private static string TrySerializeObject(object obj, out bool succeed)
    {
        try {
            // 序列化默认配置
            var jsonSerializerSettings = new JsonSerializerSettings {
                // 解决属性忽略问题
                //ContractResolver = new IgnorePropertiesContractResolver(GetIgnorePropertyNames(monitorMethod), GetIgnorePropertyTypes(monitorMethod)),

                // 解决循环引用问题
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                // 解决 DateTimeOffset 序列化/反序列化问题
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling        = DateParseHandling.None
            };

            // 解决 long 精度问题
            jsonSerializerSettings.Converters.AddLongTypeConverters();

            // 解决 DateTimeOffset 序列化/反序列化问题
            jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter {
                DateTimeStyles = DateTimeStyles.AssumeUniversal
            });

            var result = JsonConvert.SerializeObject(obj, jsonSerializerSettings);

            succeed = true;
            return result;
        }
        catch {
            succeed = true;
            return "<Error Serialize>";
        }
    }
}