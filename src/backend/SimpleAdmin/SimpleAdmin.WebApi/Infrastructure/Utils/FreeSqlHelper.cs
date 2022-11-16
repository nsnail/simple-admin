using System.Data.Common;
using System.Reflection;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using NSExt.Extensions;
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
    private FreeSqlHelper(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
        _logger          = App.GetService<ILogger<FreeSqlHelper>>();
        _user            = App.GetService<IContextUser>();
    }

    private readonly DatabaseOptions        _databaseOptions;
    private          ILogger<FreeSqlHelper> _logger;
    private          TimeSpan               _timeOffset;
    private readonly IContextUser           _user;


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
                                          .UseAutoSyncStructure(false)
                                          .Build();

        #region 数据审计

        // 获取服务器时间偏差
        var serverTime = freeSql.Ado.QuerySingle(() => DateTime.UtcNow);
        _timeOffset = DateTime.UtcNow.Subtract(serverTime);

        freeSql.Aop.AuditValue += DataAuditHandler;

        #endregion


        #region 监听Curd操作

        freeSql.Aop.CurdBefore += (_, e) => {
                                      var sql = GetNoParamSql(e.Sql, e.DbParms);
                                      _logger.Info($"SQL.{(uint)sql.GetHashCode()}：{sql}");
                                  };
        freeSql.Aop.CurdAfter += (_, e) => {
                                     var sql = GetNoParamSql(e.Sql, e.DbParms);
                                     _logger.Info($"SQL.{(uint)sql.GetHashCode()}：{e.ElapsedMilliseconds} ms");
                                 };

        #endregion 监听Curd操作

        #region 初始化数据库

        Task.Run(async () => {
                     await WaitSerilogReady();

                     var entityTypes = GetEntityTypes();


                     if (_databaseOptions.IsSyncStructure) {
                         SyncStructure(freeSql, entityTypes);
                         _logger.Info("数据库结构已同步");
                     }


                     InitSeedData(freeSql, entityTypes);
                     _logger.Info("种子数据初始化完毕");
                 });

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

    private static Type[] GetEntityTypes()
    {
        //获取所有表实体
        var entityTypes = (from type in App.EffectiveTypes
                           from attr in type.GetCustomAttributes()
                           where attr is TableAttribute { DisableSyncStructure: false }
                           select type).ToArray();
        return entityTypes;
    }

    private static string GetNoParamSql(string sql, IEnumerable<DbParameter> dbParams)
    {
        return dbParams.Where(x => x is not null)
                       .Aggregate(sql,
                                  (current, dbParam) =>
                                      current.Replace(dbParam.ParameterName, dbParam.Value?.ToString()))
                       .RemoveWrapped();
    }

    private static void InitSeedData(IFreeSql freeSql, IEnumerable<Type> entityTypes)
    {
        foreach (var entityType in entityTypes) {
            var path = $"{AppContext.BaseDirectory}/.res/seed-data/{entityType.Name}.json";
            if (!File.Exists(path)) continue;
            dynamic entities = File.ReadAllText(path).Object(typeof(List<>).MakeGenericType(entityType));
            foreach (var entity in entities) {
                var select = typeof(IFreeSql).GetMethod(nameof(freeSql.Select), 1, Type.EmptyTypes)
                                            ?.MakeGenericMethod(entityType)
                                             .Invoke(freeSql, null);
                var any = select?.GetType()
                                 .GetMethod(nameof(ISelect<dynamic>.Any), 0, Type.EmptyTypes)
                                ?.Invoke(select, null) as bool? ?? true;

                if (any) continue;
                freeSql.Insert(entity).ExecuteAffrows();
            }
        }
    }

    private void SyncStructure(IFreeSql freeSql, Type[] entityTypes)
    {
        if (_databaseOptions.DbType == DataType.Oracle) freeSql.CodeFirst.IsSyncStructureToUpper = true;
        freeSql.CodeFirst.SyncStructure(entityTypes);
    }

    private async Task WaitSerilogReady()
    {
        _logger = App.GetService<ILogger<FreeSqlHelper>>();
        for (var i = 0; i != 10; ++i) {
            if (_logger.GetType()
                       .GetRuntimeFields()
                       .FirstOrDefault(x => x.Name == "_logger")
                      ?.GetValue(_logger)
                      ?.GetType()
                       .Name == "SerilogLogger")
                break;
            await Task.Delay(100);
            _logger = App.GetService<ILogger<FreeSqlHelper>>();
        }
    }
}