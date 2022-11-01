namespace SimpleAdmin.Infrastructure.Configuration.Options.Nodes.ApiSkin;

/// <summary>
///     拦截器功能配置节点
/// </summary>
public record InterceptorFunctionsNode
{
    /// <summary>
    ///     MUST be a valid Javascript function.
    ///     Function to intercept remote definition, "Try it out", and OAuth 2.0 requests.
    ///     Accepts one argument requestInterceptor(request) and must return the modified request, or a Promise that resolves
    ///     to the modified request.
    ///     Ex: "req => { req.headers['MyCustomHeader'] = 'CustomValue'; return req; }"
    /// </summary>
    public string RequestInterceptorFunction { get; set; }

    /// <summary>
    ///     MUST be a valid Javascript function.
    ///     Function to intercept remote definition, "Try it out", and OAuth 2.0 responses.
    ///     Accepts one argument responseInterceptor(response) and must return the modified response, or a Promise that
    ///     resolves to the modified response.
    ///     Ex: "res => { console.log(res); return res; }"
    /// </summary>
    public string ResponseInterceptorFunction { get; set; }
}