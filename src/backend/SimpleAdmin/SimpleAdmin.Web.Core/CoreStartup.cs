using Furion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SimpleAdmin.Core.Constant;
using SimpleAdmin.Web.Core.Extensions;
using SimpleAdmin.Web.Core.Handlers;
using SimpleAdmin.Web.Core.Pipeline;

namespace SimpleAdmin.Web.Core;

/// <summary>
///     公共启动类
/// </summary>
[AppStartup(100)]
public class CommonStartup : AppStartup
{
    /// <summary>
    ///     配置应用程序中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app
            #if DEBUG
           .UseDeveloperExceptionPage()
            #endif


            //强制https
           .UseHttpsRedirection()
            //认证授权
           .UseAuthentication()
           .UseAuthorization()

            // 确保AspNetCore Http请求 主体可以被多次读取。
            // 修复 HttpContext 请求正文内容返回空 ，放在其他所有中间件前面
           .UseHttpRequestEnableBuffering()

            // Furion基础中间件
           .UseInject(string.Empty)

            //跨域访问中间件
           .UseCorsAccessor()
            //使用Serilog接管HTTP请求日志
           .UseSerilogRequestLogging()

            //设置识别客户端来源真实ip
           .UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            })
            //控制器路由映射
           .UseRouting()

            // 新版swagger ui（knife4j）中间件
           .UseRestSkin()

            // 配置端点
           .UseEndpoints(endpoints => { endpoints.MapControllers(); })

            //
            ;
    }

    /// <summary>
    ///     配置服务容器
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        //Jwt 授权处理器
        services.AddJwt<JwtHandler>();

        services

            //注册配置项
           .AddAllOptions()

            //支持跨域访问
           .AddCorsAccessor()
            //请求审计日志
           .AddMvcFilter<RequestAuditFilter>()

            // 远程请求
           .AddRemoteRequest()

            //注册控制器
           .AddControllers()
            // 统一controller api响应结果模板
           .AddInjectWithUnifyResult<ApiResultHandler>()
            // .AddInject()
            // ↑ ↓ 二选一
            //使用NewtonsoftJson代替asp.netcore默认System.Text.Json组件进行正反序列化
           .AddNewtonsoftJson(config => config.SerializerSettings.DateFormatString =
                                            Const.Templates.YYYY_MM_DD_HH_MM_SS);
    }
}



