using System.Reflection;
using FreeSql;
using FreeSql.Aop;
using SimpleAdmin.WebApi.Aop.Attributes;
using SimpleAdmin.WebApi.DataContracts;
using SimpleAdmin.WebApi.Infrastructure.Configuration.Options;
using SimpleAdmin.WebApi.Infrastructure.Extensions;
using Yitter.IdGenerator;

namespace SimpleAdmin.WebApi.Infrastructure.Utils;

/// <summary>
///     FreeSqlHelper
/// </summary>
public class FreeSqlHelper
{
    private readonly DatabaseOptions        _databaseOptions;
    private readonly IContextUser           _user;
    private          TimeSpan               _timeOffset;
    private readonly ILogger<FreeSqlHelper> _logger;

    private FreeSqlHelper(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
        _logger          = App.GetService<ILogger<FreeSqlHelper>>();
        _user            = App.GetService<IContextUser>();
    }


    /// <summary>
    ///     创建FreeSql
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IFreeSql Create(DatabaseOptions options)
    {
        return new FreeSqlHelper(options).Build();
    }

    private IFreeSql Build()
    {
        var freeSql = new FreeSqlBuilder().UseConnectionString(_databaseOptions.DbType, _databaseOptions.ConnStr)
                                          .UseMonitorCommand(cmd => cmd.Executing(), (cmd, log) => cmd.Executed(log))
                                          .UseAutoSyncStructure(_databaseOptions.IsSyncStructure)
                                          .Build();

        #region 数据审计

        // 获取服务器时间偏差
        var serverTime = freeSql.Ado.QuerySingle(() => DateTime.UtcNow);
        _timeOffset = DateTime.UtcNow.Subtract(serverTime);

        freeSql.Aop.AuditValue += DataAuditHandler;

        #endregion

        return freeSql;
    }

    private void DataAuditHandler(object sender, AuditValueEventArgs e)
    {
        //设置服务器时间字段
        if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) is { Enable: true }   &&
            (e.Column.CsType == typeof(DateTime) || e.Column.CsType   == typeof(DateTime?)) &&
            (e.Value         == null             || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            e.Value = DateTime.Now.Subtract(_timeOffset);

        //设置雪花id字段
        if (e.Column.CsType == typeof(long)                                              &&
            e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is { Enable: true } &&
            (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            e.Value = YitIdHelper.NextId();
    }
}