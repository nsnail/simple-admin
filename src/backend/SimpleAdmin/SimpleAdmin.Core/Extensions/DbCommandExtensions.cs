using System;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using Furion;
using Microsoft.Extensions.Logging;
using NSExt.Extensions;
using SimpleAdmin.Core.Constant;

namespace SimpleAdmin.Core.Extensions;

/// <summary>
///     DbCommand 扩展方法
/// </summary>
public static class DbCommandExtensions
{
    private static readonly Regex TABLE_NAME_REGEX = new(@"\[([a-zA-Z0-9_]+?)\]\.\[(Tb_[a-zA-Z0-9_]+?)\]",
                                                        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <summary>
    ///     sql语句执行后回调
    /// </summary>
    /// <param name="me"></param>
    /// <param name="log">执行SQL命令，返回的结果</param>
    /// <param name="commandInfo"></param>
    public static void Executed(this DbCommand                                      me,
                                string                                              log,
                                Action<Const.Enums.SqlCommandTypes, string, string> commandInfo = null)
    {
        var executeTime = log[..log.IndexOf('\n').Is(-1, log.Length)];
        // 输出sql日志到日志管道
        App.GetService<ILogger<DbCommand>>()?.Debug(executeTime);


        //通知
        if (commandInfo is null) return;
        var firstOrder = Enum.GetNames<Const.Enums.SqlCommandTypes>()
                             .Select(x => (x, me.CommandText.IndexOf(x, StringComparison.OrdinalIgnoreCase)))
                             .Where(x => x.Item2 >= 0)
                             .OrderBy(x => x.Item2)
                             .FirstOrDefault();
        if (!Enum.TryParse(typeof(Const.Enums.SqlCommandTypes), firstOrder.x, true, out var typeObject)) return;

        var type       = (Const.Enums.SqlCommandTypes)typeObject!;
        var schemaName = TABLE_NAME_REGEX.Match(me.CommandText).Groups[1].Value;
        var tableName  = TABLE_NAME_REGEX.Match(me.CommandText).Groups[2].Value;
        commandInfo(type, schemaName, tableName);
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