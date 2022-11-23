using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.HttpOverrides;
using NSExt.Extensions;
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
           .UseDeveloperExceptionPage() //                             开发者异常信息页
            #else
           .UseHttpsRedirection() //                                   强制https
            #endif


           .UseAuthentication() //                                     认证授权
           .UseAuthorization()  //                                     认证授权
            // ↓                                                       修复 HttpContext  请求正文内容返回空 ，放在其他所有中间件前面
           .UseHttpRequestEnableBuffering() //                         确保AspNetCore Http请求 主体可以被多次读取。
           .UseInject(string.Empty)         //                         Furion基础中间件
           .UseCorsAccessor()               //                         跨域访问中间件
           .UseSerilogRequestLogging()      //                         使用Serilog接管HTTP请求日志
           .UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            })               //                                        设置识别客户端来源真实ip
           .UseRouting()     //                                        控制器路由映射
           .UseSwaggerSkin() //                                        新版swagger ui（knife4j）中间件
           .UseEndpoints(endpoints =>
                             //
                             endpoints.MapControllers()) //            配置端点
            ;
    }

    /// <summary>
    ///     配置服务容器
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddJwt<JwtHandler>(enableGlobalAuthorize: true) //    Jwt 授权处理器
                .Services
                 #if DEBUG
                .AddMonitorLogging() //                                打印日志监视信息，便于调试
                 #endif

                .AddMvcFilter<RequestAuditHandler>()          //       请求审计日志
                .AddSnowflake()                               //       雪花id生成器
                .AddEventBus()                                //       事件总线
                .AddFreeSql()                                 //       注册freeSql
                .AddRedis()                                   //       注册redis
                .AddAllOptions()                              //       注册配置项
                .AddCorsAccessor()                            //       支持跨域访问
                .AddRemoteRequest()                           //       远程请求
                .AddControllers()                             //       注册控制器
                .AddInjectWithUnifyResult<ApiResultHandler>() //       api响应结果模板
                .AddJsonOptions(options =>
                                    options.JsonSerializerOptions.CopyFrom(default(JsonSerializerOptions)
                                                                              .NewJsonSerializerOptions())
                                //
                               ) //                                    json序列化配置

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
        PrintLog();

        Serve.Run(RunOptions.Default.WithArgs(args)
                            .ConfigureBuilder(builder =>
                                                  //
                                                  builder.UseSerilogDefault(config => config.Init())
                                              //
                                             ));
    }


    private static void PrintLog()
    {
        var asm     = typeof(Startup).Assembly;
        var version = $" v{asm.GetName().Version} ";
        var repUrl = $" {asm.GetCustomAttributes<AssemblyMetadataAttribute>()
                            .FirstOrDefault(x => x.Key == "RepositoryUrl")
                           ?.Value} ";

        var logo = (t: """
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
""", w: 67, c: '─');

        Console.WriteLine(logo.t,
                          version.PadLeft((logo.w + version.Length) / 2, logo.c).PadRight(logo.w, logo.c),
                          repUrl.PadLeft((logo.w  + repUrl.Length)  / 2, logo.c).PadRight(logo.w, logo.c));
    }
}