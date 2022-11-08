using System.Reflection;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using NSExt.Extensions;
using SimpleAdmin.DataContract.DbMaps.Dependency;
using SimpleAdmin.Infrastructure.Attributes;
using SimpleAdmin.Infrastructure.Configuration.Options.Nodes.Connection;
using SimpleAdmin.Infrastructure.Constant;
using SimpleAdmin.Infrastructure.Extensions;
using SimpleAdmin.Infrastructure.Identity;
using StackExchange.Profiling.Internal;
using Yitter.IdGenerator;

namespace SimpleAdmin.WebApi.Utils;

public static class FreeSqlHelper
{
    private static void CreateDatabase(ServersNode server)
    {
        using var db = new FreeSqlBuilder().UseConnectionString(server.DbType, server.ConnStr).Build();

        var filePath = Path.Combine(AppContext.BaseDirectory, server.CreateDbFilePath);
        if (!File.Exists(filePath)) return;

        var createDbSql = File.ReadAllText(filePath);
        var logger      = App.GetService<ILogger<Startup>>();
        logger.Info("建库开始");
        try {
            db.Ado.ExecuteNonQuery(createDbSql);
            logger.Info("建库完成");
        }
        catch (Exception ex) {
            logger.Error($"建库失败：{ex}");
        }
    }

    public static async Task<IFreeSql> Register(ServersNode                                         server,
                                                Action<Const.Enums.SqlCommandTypes, string, string> commandInfo = null)
    {
        if (!server.CreateDbFilePath.IsNullOrWhiteSpace()) CreateDatabase(server);

        var freeSql = new FreeSqlBuilder().UseConnectionString(server.DbType, server.ConnStr)
                                          .UseMonitorCommand(cmd => cmd.Executing(),
                                                             (cmd, log) => cmd.Executed(log, commandInfo))
                                          .UseAutoSyncStructure(server.IsSyncStructure)
                                          .Build();

        var user = App.GetService<IContextUser>();

        // 过滤器
        {
            //软删除过滤器
            freeSql.GlobalFilter.ApplyOnly<IEntityDelete>(nameof(Const.Enums.FreeSqlGlobalFilters.Delete),
                                                          a => a.IsDeleted == false);

            //租户过滤器


            if (server.IsMultiTenant)
                freeSql.GlobalFilter.ApplyOnlyIf<ITenant>(nameof(Const.Enums.FreeSqlGlobalFilters.Tenant),
                                                          () => user?.Id  > 0,
                                                          a => a.TenantId == user.TenantId);


            //数据权限过滤器
            freeSql.GlobalFilter.ApplyOnlyIf<IData>(nameof(Const.Enums.FreeSqlGlobalFilters.Self),
                                                    () => {
                                                        if (!(user?.Id > 0)) return false;
                                                        var dataPermission = user.DataPermission;
                                                        if (user.Type      == Const.Enums.UserTypes.DefaultUser &&
                                                            dataPermission != null)
                                                            return dataPermission.DataScopes !=
                                                                   Const.Enums.DataScopes.All &&
                                                                   dataPermission.OrgIds.Count == 0;
                                                        return false;
                                                    },
                                                    a => a.OwnerId == user.Id);
            freeSql.GlobalFilter.ApplyOnlyIf<IData>(nameof(Const.Enums.FreeSqlGlobalFilters.Data),
                                                    () => {
                                                        if (!(user?.Id > 0)) return false;
                                                        var dataPermission = user.DataPermission;
                                                        if (user.Type      == Const.Enums.UserTypes.DefaultUser &&
                                                            dataPermission != null)
                                                            return dataPermission.DataScopes !=
                                                                   Const.Enums.DataScopes.All &&
                                                                   dataPermission.OrgIds.Count > 0;
                                                        return false;
                                                    },
                                                    a => a.OwnerId == user.Id ||
                                                         user.DataPermission.OrgIds.Contains(a.OwnerOrgId.Value));
        }

        //配置实体
        if (server.IsMultiTenant) {
            //获得指定程序集表实体
            var entityTypes = (from type in App.EffectiveTypes
                               from attr in type.GetCustomAttributes()
                               where attr is TableAttribute { DisableSyncStructure: false }
                               select type).ToList();

            foreach (var entityType in entityTypes.Where(entityType =>
                                                             entityType.GetInterfaces()
                                                                       .Any(x => x.Name == nameof(ITenant))))
                freeSql.CodeFirst.Entity(entityType, x => { x.Ignore(nameof(ITenant.TenantId)); });
        }

        // 初始化数据库
        {
            //计算服务器时间
            var serverTime = freeSql.Ado.QuerySingle(() => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            _timeOffset = timeOffset;

            // 审计数据
            freeSql.Aop.AuditValue += (_, e) => { AuditValue(e, timeOffset, user); };


            //同步数据
            if (server.IsSyncData) await SyncDataAsync(freeSql, server);
        }

        return freeSql;
    }

    private static TimeSpan _timeOffset;

    /// <summary>
    ///     同步数据审计方法
    /// </summary>
    /// <param name="s"></param>
    /// <param name="e"></param>
    private static void SyncDataAuditValue(object s, AuditValueEventArgs e)
    {
        var user = new {
            Id       = 161223411986501,
            Name     = "admin",
            TenantId = 161223412138053
        };

        if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null               &&
            (e.Column.CsType == typeof(DateTime) || e.Column.CsType   == typeof(DateTime?)) &&
            (e.Value         == null             || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            e.Value = DateTime.Now.Subtract(_timeOffset);

        if (e.Column.CsType == typeof(long) && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null &&
            (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            e.Value = YitIdHelper.NextId();

        if (user.Id <= 0) return;

        switch (e.AuditValueType) {
            case AuditValueType.Insert:
                switch (e.Property.Name) {
                    case "CreatedUserId":
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default) e.Value = user.Id;
                        break;

                    case "CreatedUserName":
                        if (e.Value == null || ((string)e.Value).IsNullOrWhiteSpace()) e.Value = user.Name;
                        break;

                    case "TenantId":
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                            e.Value = user.TenantId;
                        break;
                }

                break;
            case AuditValueType.Update:
                e.Value = e.Property.Name switch {
                              "ModifiedUserId"   => user.Id,
                              "ModifiedUserName" => user.Name,
                              _                  => e.Value
                          };
                break;
            case AuditValueType.InsertOrUpdate:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    private static async Task SyncDataAsync(IFreeSql freeSql, ServersNode server)
    {
        try {
            Console.WriteLine($"{Environment.NewLine} sync data started");

            freeSql.Aop.AuditValue += SyncDataAuditValue;

            var assemblies = App.Assemblies;
            var syncDatas = assemblies.Select(assembly => assembly.GetTypes()
                                                                  .Where(x => typeof(ISyncData).GetTypeInfo()
                                                                                 .IsAssignableFrom(x.GetTypeInfo()) &&
                                                                              x.GetTypeInfo().IsClass               &&
                                                                              !x.GetTypeInfo().IsAbstract))
                                      .SelectMany(registerTypes =>
                                                      registerTypes.Select(registerType =>
                                                                               (ISyncData)Activator
                                                                                  .CreateInstance(registerType)))
                                      .ToList();

            foreach (var syncData in syncDatas) await syncData.SyncDataAsync(freeSql, server);

            freeSql.Aop.AuditValue -= SyncDataAuditValue;


            Console.WriteLine($" sync data succeed{Environment.NewLine}");
        }
        catch (Exception ex) {
            throw new Exception($" sync data failed.\n{ex.Message}");
        }
    }

    /// <summary>
    ///     审计数据
    /// </summary>
    /// <param name="e"></param>
    /// <param name="timeOffset"></param>
    /// <param name="user"></param>
    public static void AuditValue(AuditValueEventArgs e, TimeSpan timeOffset, IContextUser user)
    {
        if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null               &&
            (e.Column.CsType == typeof(DateTime) || e.Column.CsType   == typeof(DateTime?)) &&
            (e.Value         == null             || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            e.Value = DateTime.Now.Subtract(timeOffset);

        if (e.Column.CsType == typeof(long)                                              &&
            e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is { Enable: true } &&
            (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            e.Value = YitIdHelper.NextId();

        if (user is not { Id: > 0 }) return;

        switch (e.AuditValueType) {
            case AuditValueType.Insert:
                switch (e.Property.Name) {
                    case nameof(EntityData.OwnerId):
                    case nameof(EntityBase.CreatedUserId):
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default) e.Value = user.Id;
                        break;

                    case nameof(EntityBase.CreatedUserName):
                        if (e.Value == null || ((string)e.Value).IsNullOrWhiteSpace()) e.Value = user.UserName;
                        break;
                    case nameof(EntityData.OwnerOrgId):
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                            e.Value = user.DataPermission?.OrgId;
                        break;
                    case nameof(EntityTenant.TenantId):
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                            e.Value = user.TenantId;
                        break;
                }

                break;
            case AuditValueType.Update:
                e.Value = e.Property.Name switch {
                              nameof(EntityBase.ModifiedUserId)   => user.Id,
                              nameof(EntityBase.ModifiedUserName) => user.UserName,
                              _                                   => e.Value
                          };

                break;
            case AuditValueType.InsertOrUpdate:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}