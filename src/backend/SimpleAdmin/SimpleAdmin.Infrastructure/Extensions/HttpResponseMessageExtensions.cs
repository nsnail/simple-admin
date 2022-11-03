using Microsoft.Extensions.Logging;
using NSExt.Extensions;

namespace SimpleAdmin.Infrastructure.Extensions;

/// <summary>
///     HttpResponseMessage 扩展方法
/// </summary>
public static class HttpResponseMessageExtensions
{
    /// <summary>
    ///     将Http请求的Uri、Header、Body打包成Json字符串
    /// </summary>
    /// <param name="me"></param>
    /// <param name="bodyHandle"></param>
    /// <returns></returns>
    public static async Task<string> BuildJson(this HttpResponseMessage me, Func<string, string> bodyHandle = null)
    {
        var body = me?.Content is null ? null : await me.Content!.ReadAsStringAsync();
        return new {
            Header = me?.ToString(),
            Body   = bodyHandle is null ? body : bodyHandle(body)
        }.Json();
    }

    /// <summary>
    ///     记录日志
    /// </summary>
    /// <param name="me"></param>
    /// <param name="logger"></param>
    /// <param name="bodyPreHandle">body预处理器</param>
    /// <typeparam name="T"></typeparam>
    public static async Task Log<T>(this HttpResponseMessage me,
                                    ILogger<T>               logger,
                                    Func<string, string>     bodyPreHandle = null)
    {
        logger.Info($"响应：{await me.BuildJson(bodyPreHandle)}");
    }

    /// <summary>
    ///     记录日常日志
    /// </summary>
    /// <param name="me"></param>
    /// <param name="errors"></param>
    /// <param name="logger"></param>
    /// <param name="bodyHandle"></param>
    /// <typeparam name="T"></typeparam>
    public static async Task LogException<T>(this HttpResponseMessage me,
                                             string                   errors,
                                             ILogger<T>               logger,
                                             Func<string, string>     bodyHandle = null)
    {
        logger.Error($"异常（{errors}）：{await me.BuildJson(bodyHandle)}");
    }
}