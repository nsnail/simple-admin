using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using NSExt.Extensions;
using SimpleAdmin.Infrastructure.Configuration.Options;

namespace SimpleAdmin.WebApi.Middlewares;

/// <summary>
///     Api 界面 knife4j-vue 中间件
/// </summary>
public class RestSkinMiddleware
{
    private readonly IWebHostEnvironment _env;
    private readonly ILoggerFactory      _logger;
    private readonly RequestDelegate     _next;

    private readonly RestSkinOptions      _options;
    private readonly StaticFileMiddleware _staticFileMiddleware;

    /// <summary>
    ///     Api 界面 knife4j-vue 中间件
    /// </summary>
    /// <param name="next">下一个中间件</param>
    /// <param name="env">主机环境</param>
    /// <param name="logger">日志工厂</param>
    /// <param name="options">api skin 配置项</param>
    public RestSkinMiddleware(RequestDelegate           next,
                              IWebHostEnvironment       env,
                              ILoggerFactory            logger,
                              IOptions<RestSkinOptions> options)
    {
        _next                 = next;
        _env                  = env;
        _logger               = logger;
        _options              = options.Value;
        _staticFileMiddleware = CreateStaticFileMiddleware();
    }


    /// <summary>
    ///     创建静态文件处理中间件
    /// </summary>
    /// <returns></returns>
    private StaticFileMiddleware CreateStaticFileMiddleware()
    {
        var staticFileOptions = new StaticFileOptions {
            RequestPath  = string.IsNullOrWhiteSpace(_options.RoutePrefix) ? string.Empty : $"/{_options.RoutePrefix}",
            FileProvider = new EmbeddedFileProvider(GetType().Assembly, EMBEDDED_FILE_NAMESPACE)
        };

        return new StaticFileMiddleware(_next, _env, Options.Create(staticFileOptions), _logger);
    }

    private const string EMBEDDED_FILE_NAMESPACE = $"{nameof(SimpleAdmin)}.{nameof(WebApi)}.skin.dist";

    /// <summary>
    ///     替换字典（首页）
    /// </summary>
    /// <returns></returns>
    private IDictionary<string, string> GetIndexArguments()
    {
        return new Dictionary<string, string> {
            { "%(DocumentTitle)", _options.DocumentTitle },
            { "%(HeadContent)", _options.HeadContent },
            { "%(ConfigObject)", _options.ConfigObject.JsonCamelCase() },
            { "%(OAuthConfigObject)", _options.OAuthConfigObject.JsonCamelCase() },
            { "%(Interceptors)", _options.Interceptors.JsonCamelCase() }
        };
    }

    private const string INDEX_FILE_NAME = "index.html";

    /// <summary>
    ///     中间件主处理器
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value!;

        switch (context.Request.Method) {
            // 以前缀结尾则 重定向到首页
            case "GET" when
                Regex.IsMatch(path, $"^/?{Regex.Escape(_options.RoutePrefix)}/?$", RegexOptions.IgnoreCase): {
                // Use relative redirect to support proxy environments
                var relativeIndexUrl = string.IsNullOrWhiteSpace(path) || path.EndsWith("/")
                                           ? INDEX_FILE_NAME
                                           : $"{path.Split('/').Last()}/{INDEX_FILE_NAME}";

                RespondWithRedirect(context.Response, relativeIndexUrl);
                return;
            }
            // 响应首页
            case "GET" when Regex.IsMatch(path,
                                          $"^/{Regex.Escape(_options.RoutePrefix)}/?{INDEX_FILE_NAME}$",
                                          RegexOptions.IgnoreCase):
                await RespondWithIndexHtml(context.Response);
                return;
            // 前端特殊用途
            case "GET" when Regex.IsMatch(path, "^/swagger-resources$", RegexOptions.IgnoreCase):
                // 前端特殊用途： knife4j -vue / Knife4jAsync.js line 74： 此处判断底层springfox版本
                // 1、springfox提供的分组地址   /swagger -resources
                // 2、springdoc                   -open提供的分组地址：v3 /api -docs /swagger -config
                // swagger请求api地址
                // if(this.springdoc){
                //     this.url = options.url || 'v3/api-docs/swagger-config'
                // }else{
                //     this.url = options.url || 'swagger-resources'
                // }
                await context.Response.WriteAsync(_options.ConfigObject.Urls.JsonCamelCase());
                return;
            // 响应其他资源
            default:
                await _staticFileMiddleware.Invoke(context);
                break;
        }
    }

    /// <summary>
    ///     读取资源文件（首页），替换字典，输出到 HttpResponse
    /// </summary>
    /// <param name="response"></param>
    private async Task RespondWithIndexHtml(HttpResponse response)
    {
        response.StatusCode  = 200;
        response.ContentType = "text/html;charset=utf-8";

        await using var stream =
            GetType().Assembly.GetManifestResourceStream($"{EMBEDDED_FILE_NAMESPACE}.{INDEX_FILE_NAME}");
        // Inject arguments before writing to response
        var htmlBuilder = new StringBuilder(await new StreamReader(stream!).ReadToEndAsync());
        foreach (var entry in GetIndexArguments()) htmlBuilder.Replace(entry.Key, entry.Value);

        await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
    }

    /// <summary>
    ///     url 重定向
    /// </summary>
    /// <param name="response"></param>
    /// <param name="location"></param>
    private static void RespondWithRedirect(HttpResponse response, string location)
    {
        response.StatusCode          = 301;
        response.Headers["Location"] = location;
    }
}