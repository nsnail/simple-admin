using System.Reflection;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using SimpleAdmin.WebApi.Aop.Filters;
using SimpleAdmin.WebApi.Infrastructure.Extensions;

namespace SimpleAdmin.WebApi;

/// <summary>
///     启动类
/// </summary>
[AppStartup(100)]
public class Startup : AppStartup
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


            #if !DEBUG
            //强制https
           .UseHttpsRedirection()
            #endif

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
           .UseSwaggerSkin()

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
        services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);

        services
            #if DEBUG
            // 打印日志监视信息，便于调试
           .AddMonitorLogging()
            #endif

            //请求审计日志
           .AddMvcFilter<RequestAuditHandler>()

            // 雪花id生成器
           .AddSnowflake()

            //事件总线
           .AddEventBus()

            // 注册freeSql
           .AddFreeSql()

            //注册redis
           .AddRedis()

            //注册配置项
           .AddAllOptions()

            //支持跨域访问
           .AddCorsAccessor()

            // 远程请求
            // .AddRemoteRequest()

            //注册控制器
           .AddControllers()
            // 统一controller api响应结果模板
           .AddInjectWithUnifyResult<ApiResultHandler>()

            //使用NewtonsoftJson代替asp.netcore默认System.Text.Json组件进行正反序列化
            // .AddNewtonsoftJson(config => {
            //                        #region 序列化设置
            //
            //                        // 日期格式
            //                        config.SerializerSettings.DateFormatString = Strings.FMT_YYYY_MM_DD_HH_MM_SS;
            //                        // 小驼峰属性名
            //                        config.SerializerSettings.ContractResolver =
            //                            new CamelCasePropertyNamesContractResolver();
            //
            //                        #endregion
            //                    })
            //
            ;
    }

    /// <summary>
    ///     程序入口
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        const string logo = """
┌{0}┐
│                                                                   │
│   _____ _                 _                  _           _        │
│  / ____(_)               | |        /\      | |         (_)       │
│ | (___  _ _ __ ___  _ __ | | ___   /  \   __| |_ __ ___  _ _ __   │
│  \___ \| | '_ ` _ \| '_ \| |/ _ \ / /\ \ / _` | '_ ` _ \| | '_ \  │
│  ____) | | | | | | | |_) | |  __// ____ \ (_| | | | | | | | | | | │
│ |_____/|_|_| |_| |_| .__/|_|\___/_/    \_\__,_|_| |_| |_|_|_| |_| │
│                    | |                                            │
│                    |_|                                            │
│                                                                   │
└{1}┘
""";


        const int  width   = 67;
        const char padChar = '─';
        var        asm     = typeof(Startup).Assembly;
        var        version = $" v{asm.GetName().Version} ";
        var repUrl = $" {asm.GetCustomAttributes<AssemblyMetadataAttribute>()
                            .FirstOrDefault(x => x.Key == "RepositoryUrl")
                           ?.Value} ";


        Console.WriteLine(logo,
                          version.PadLeft((width + version.Length) / 2, padChar).PadRight(width, padChar),
                          repUrl.PadLeft((width  + repUrl.Length)  / 2, padChar).PadRight(width, padChar));


        Serve.Run(RunOptions.Default.WithArgs(args)
                            .ConfigureBuilder(builder =>
                                                  //
                                                  builder.UseSerilogDefault(config => config.Init())
                                              //
                                             ));
    }
}