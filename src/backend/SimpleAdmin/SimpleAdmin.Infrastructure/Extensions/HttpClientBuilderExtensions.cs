using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleAdmin.Infrastructure.Extensions;

/// <summary>
///     IHttpClientBuilder 扩展方法
/// </summary>
public static class HttpClientBuilderExtensions
{
    /// <summary>
    ///     使用Fiddler代理（抓包调试）
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static IHttpClientBuilder AddFiddler(this IHttpClientBuilder me)
    {
        return me.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
            UseProxy                = true,
            Proxy                   = new WebProxy("127.0.0.1", 8888), // use system proxy
            DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials
        });
    }
}