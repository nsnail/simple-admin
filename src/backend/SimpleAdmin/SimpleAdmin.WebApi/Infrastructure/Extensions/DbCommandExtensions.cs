using System.Data.Common;
using NSExt.Extensions;

namespace SimpleAdmin.WebApi.Infrastructure.Extensions;

/// <summary>
///     DbCommand 扩展方法
/// </summary>
public static class DbCommandExtensions
{
    /// <summary>
    ///     sql语句执行后回调
    /// </summary>
    /// <param name="me"></param>
    /// <param name="log">执行SQL命令，返回的结果</param>
    public static void Executed(this DbCommand me, string log)
    {
        var executeTime = log[..log.IndexOf('\n').Is(-1, log.Length)];
        // 输出sql日志到日志管道
        App.GetService<ILogger<DbCommand>>()?.Debug(executeTime);
    }

    /// <summary>
    ///     sql语句执行前回调
    /// </summary>
    /// <param name="me"></param>
    public static void Executing(this DbCommand me)
    {
        var sqlText = me.ParameterFormat();
        // 输出sql日志到miniProfiler
        // 输出sql日志到日志管道
        App.GetService<ILogger<DbCommand>>()?.Debug(sqlText);
    }
}