using System.Text;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.SystemConsole.Themes;
using SimpleAdmin.Infrastructure.Constant;

// ReSharper disable UnusedMember.Global

namespace SimpleAdmin.Infrastructure.Extensions;

/// <summary>
///     Serilog配置类扩展
/// </summary>
public static class LoggerConfigurationExtensions
{
    /// <summary>
    ///     以日志来源为文件名配置文件日志
    /// </summary>
    /// <param name="config"></param>
    /// <param name="sourceName"></param>
    public static void FileLogSaveBySource(this LoggerConfiguration config, string sourceName)
    {
        config.WriteTo.Logger(subConfig => {
                                  subConfig.Filter.ByIncludingOnly(Matching.FromSource(sourceName));
                                  subConfig.WriteTo.File($"logs/{sourceName}/.log",
                                                         outputTemplate: Const.Templates.LOG_OUTPUT_TEMPLATE_FULL,
                                                         rollingInterval: RollingInterval.Day,
                                                         encoding: Encoding.UTF8);
                              });
    }

    /// <summary>
    ///     以日志类型为文件名配置文件日志
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="config"></param>
    public static void FileLogSaveByType<T>(this LoggerConfiguration config)
    {
        FileLogSaveBySource(config, typeof(T).FullName);
    }

    /// <summary>
    ///     初始化Serilog日志组件配置
    /// </summary>
    /// <param name="me"></param>
    public static LoggerConfiguration Init(this LoggerConfiguration me)
    {
        //	//日志按日保存，这样会在文件名称后自动加上日期后缀
        //	, rollingInterval: RollingInterval.Day
        //	//,rollOnFileSizeLimit: false          // 限制单个文件的最大长度
        //	//,retainedFileCountLimit: 10,         // 最大保存文件数,等于null时永远保留文件。
        //	//,fileSizeLimitBytes: 10 * 1024,      // 最大单个文件大小
        //	, encoding: Encoding.UTF8 // 文件字符编码


        #region 全部日志

        //写入控制台
        me.WriteTo.Console(outputTemplate: Const.Templates.LOG_OUTPUT_TEMPLATE_FULL,
                           theme: new SystemConsoleTheme(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle> {
                               [ConsoleThemeStyle.Text] = new() {
                                   Foreground = ConsoleColor.White
                               },
                               [ConsoleThemeStyle.SecondaryText] = new() {
                                   Foreground = ConsoleColor.Gray
                               },
                               [ConsoleThemeStyle.TertiaryText] = new() {
                                   Foreground = ConsoleColor.DarkGray
                               },
                               [ConsoleThemeStyle.Invalid] = new() {
                                   Foreground = ConsoleColor.Yellow
                               },
                               [ConsoleThemeStyle.Null] = new() {
                                   Foreground = ConsoleColor.Blue
                               },
                               [ConsoleThemeStyle.Name] = new() {
                                   Foreground = ConsoleColor.Gray
                               },
                               [ConsoleThemeStyle.String] = new() {
                                   Foreground = ConsoleColor.Cyan
                               },
                               [ConsoleThemeStyle.Number] = new() {
                                   Foreground = ConsoleColor.Magenta
                               },
                               [ConsoleThemeStyle.Boolean] = new() {
                                   Foreground = ConsoleColor.Blue
                               },
                               [ConsoleThemeStyle.Scalar] = new() {
                                   Foreground = ConsoleColor.Green
                               },
                               [ConsoleThemeStyle.LevelVerbose] = new() {
                                   Foreground = ConsoleColor.Gray
                               },
                               [ConsoleThemeStyle.LevelDebug] = new() {
                                   Foreground = ConsoleColor.White
                               },
                               [ConsoleThemeStyle.LevelInformation] = new() {
                                   Foreground = ConsoleColor.Green
                               },
                               [ConsoleThemeStyle.LevelWarning] = new() {
                                   Foreground = ConsoleColor.Yellow
                               },
                               [ConsoleThemeStyle.LevelError] = new() {
                                   Foreground = ConsoleColor.White,
                                   Background = ConsoleColor.Red
                               },
                               [ConsoleThemeStyle.LevelFatal] = new() {
                                   Foreground = ConsoleColor.White,
                                   Background = ConsoleColor.Red
                               }
                           }));
        Console.OutputEncoding = Encoding.UTF8;

        //写入文件
        // me.WriteTo.File("logs/.log",
        //     outputTemplate: Const.LOG_OUTPUT_TEMPLATE,
        //     rollingInterval: RollingInterval.Day,
        //     encoding: Encoding.UTF8);

        #endregion

        // LogEventLevel.Warning 类型级别以上 写入文件
        me.WriteTo.File("logs/.log",
                        outputTemplate: Const.Templates.LOG_OUTPUT_TEMPLATE_FULL,
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: LogEventLevel.Warning,
                        encoding: Encoding.UTF8);


        // LogEventLevel.Error 类型级别以上保存到数据库
        // me.WriteTo.Logger(subConfig => {
        //                       subConfig.Filter.ByIncludingOnly(x => x.Level >= LogEventLevel.Error);
        //                       subConfig.WriteTo.MSSqlServer(
        //                                                         App.GetOptions<ConnectionsOptions>()
        //                                                            .SerilogConnection
        //
        //                                                    ,
        //                                                     new MSSqlServerSinkOptions {
        //                                                         TableName          = "LogTasks",
        //                                                         AutoCreateSqlTable = true
        //                                                     });
        //                   });
        return me;
    }
}