namespace SimpleAdmin.WebApi.Middlewares;

/// <summary>
///     确保AspNetCore Http请求 主体可以被多次读取。
/// </summary>
public class HttpRequestEnableBufferingMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    ///     确保AspNetCore Http请求 主体可以被多次读取。
    /// </summary>
    /// <param name="next">下一个中间件指针</param>
    public HttpRequestEnableBufferingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///     中间件主处理器
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        // 调用下一个中间件
        await _next(context);
    }
}