using SimpleAdmin.Infrastructure.Configuration.Options.Nodes.ApiSkin;

namespace SimpleAdmin.Infrastructure.Configuration.Options;

/// <summary>
///     API 界面 knife4j-vue 配置
/// </summary>
public record RestSkinOptions : OptionAbstraction
{
    /// <summary>
    ///     swagger 配置对象 节点
    ///     Gets the JavaScript config object, represented as JSON, that will be passed to the SwaggerUI
    /// </summary>
    public ConfigObjectNode ConfigObject { get; set; } = new();

    /// <summary>
    ///     文档标题
    ///     Gets or sets a title for the swagger-ui page
    /// </summary>
    public string DocumentTitle { get; set; } = "api skin";

    /// <summary>
    ///     自定义页面 head 节点
    ///     Gets or sets additional content to place in the head of the swagger-ui page
    /// </summary>
    public string HeadContent { get; set; } = "";


    /// <summary>
    ///     request & response 拦截器
    ///     Gets the interceptor functions that define client-side request/response interceptors
    /// </summary>
    public InterceptorFunctionsNode Interceptors { get; set; } = new();

    /// <summary>
    ///     OAuth 配置节点
    ///     Gets the JavaScript config object, represented as JSON, that will be passed to the initOAuth method
    /// </summary>
    public OAuthConfigObjectNode OAuthConfigObject { get; set; } = new();

    /// <summary>
    ///     当url path 以此值开头，则交给 api skin 中间件处理 ， 开头不需要 "/"
    ///     Gets or sets a route prefix for accessing the swagger-ui
    /// </summary>
    public string RoutePrefix { get; set; } = "";
}