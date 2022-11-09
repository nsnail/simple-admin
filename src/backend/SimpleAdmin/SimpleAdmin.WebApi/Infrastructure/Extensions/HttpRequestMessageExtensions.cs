using NSExt.Extensions;

namespace SimpleAdmin.WebApi.Infrastructure.Extensions;

/// <summary>
///     HttpRequestMessage 扩展方法
/// </summary>
public static class HttpRequestMessageExtensions
{
    /// <summary>
    ///     将Http请求的Uri、Header、Body打包成Json字符串
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static async Task<string> BuildJson(this HttpRequestMessage me)
    {
        var body = me?.Content is null ? null : await me.Content!.ReadAsStringAsync();
        return new {
            Uri    = me?.RequestUri,
            Header = me?.ToString(),
            Body   = body
        }.Json();
    }

    /// <summary>
    ///     记录日志
    /// </summary>
    /// <param name="me"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T"></typeparam>
    public static async Task Log<T>(this HttpRequestMessage me, ILogger<T> logger)
    {
        logger.Info($"请求：{await me.BuildJson()}");
    }
}