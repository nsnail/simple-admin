using System;
using System.Net.Http;
using Furion.RemoteRequest;
using Microsoft.Extensions.Logging;

namespace SimpleAdmin.Core.Extensions;

/// <summary>
///     HttpRequestPart 扩展方法
/// </summary>
public static class HttpRequestPartExtensions
{
    /// <summary>
    ///     设置日志
    /// </summary>
    /// <param name="me"></param>
    /// <param name="logger"></param>
    /// <param name="bodyHandle"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static HttpRequestPart SetLog<T>(this HttpRequestPart me,
                                            ILogger<T>           logger,
                                            Func<string, string> bodyHandle = null)
    {
        async void RequestHandle(HttpClient client, HttpRequestMessage req)
        {
            await req.Log(logger);
        }

        async void ResponseHandle(HttpClient client, HttpResponseMessage rsp)
        {
            await rsp.Log(logger, bodyHandle);
        }

        async void ExceptionHandle(HttpClient client, HttpResponseMessage rsp, string errors)
        {
            await rsp.LogException(errors, logger, bodyHandle);
        }

        return me.OnRequesting(RequestHandle).OnResponsing(ResponseHandle).OnException(ExceptionHandle);
    }
}



