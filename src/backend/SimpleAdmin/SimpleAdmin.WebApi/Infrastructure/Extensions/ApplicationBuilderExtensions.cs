using SimpleAdmin.WebApi.Aop.Middlewares;

namespace SimpleAdmin.WebApi.Infrastructure.Extensions;

/// <summary>
///     ApplicationBuilder对象 扩展方法
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    ///     确保AspNetCore Http请求 主体可以被多次读取。
    /// </summary>
    public static IApplicationBuilder UseHttpRequestEnableBuffering(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HttpRequestEnableBufferingMiddleware>();
    }

    /// <summary>
    ///     使用 api skin （knife4j-vue）
    /// </summary>
    public static IApplicationBuilder UseSwaggerSkin(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SwaggerSkinMiddleware>();
    }
}