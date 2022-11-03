using System.Reflection;
using FreeSql;
using Furion.ConfigurableOptions;
using NSExt.Extensions;
using SimpleAdmin.DataContract.DbMaps.Dependency;
using SimpleAdmin.Infrastructure.Configuration.Options;
using SimpleAdmin.Infrastructure.Configuration.Options.Nodes.Connection;
using SimpleAdmin.Infrastructure.Constant;
using SimpleAdmin.Infrastructure.Extensions;
using SimpleAdmin.Infrastructure.Identity;
using SimpleAdmin.WebApi.AopHooks;
using StackExchange.Profiling.Internal;

namespace SimpleAdmin.WebApi.Extensions;

/// <summary>
///     ServiceCollection 扩展方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     扫描程序集中继承自IConfigurableOptions的选项，注册
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static IServiceCollection AddAllOptions(this IServiceCollection me)
    {
        var optionsTypes =
            from type in App.EffectiveTypes.Where(x => !x.FullName!.Contains(nameof(Furion)) &&
                                                       x.GetInterfaces().Contains(typeof(IConfigurableOptions)))
            select type;

        foreach (var type in optionsTypes) {
            var configureMethod =
                typeof(ConfigurableOptionsServiceCollectionExtensions).GetMethod(nameof(
                        ConfigurableOptionsServiceCollectionExtensions.AddConfigurableOptions),
                    BindingFlags.Public | BindingFlags.Static,
                    new[] {
                        typeof(IServiceCollection)
                    });
            configureMethod!.MakeGenericMethod(type)
                            .Invoke(me,
                                    new object[] {
                                        me
                                    });
        }

        return me;
    }


    /// <summary>
    ///     注册freeSql orm工具
    /// </summary>
    /// <param name="me"></param>
    /// <param name="name"></param>
    /// <param name="commandInfo"></param>
    /// <returns></returns>
    public static IServiceCollection AddFreeSql(this IServiceCollection                             me,
                                                string                                              name,
                                                Action<Const.Enums.SqlCommandTypes, string, string> commandInfo = null)
    {
        void CreateDatabase(ServersNode server)
        {
            using var db = new FreeSqlBuilder().UseConnectionString(Enum.Parse<DataType>(server.DbType), server.ConnStr)
                                               .Build();

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


        var server = App.GetConfig<ConnectionsOptions>(nameof(ConnectionsOptions).TrimEndOptions())[name];

        if (!server.CreateDbFilePath.IsNullOrWhiteSpace()) CreateDatabase(server);

        var freeSql = new FreeSqlBuilder().UseConnectionString(Enum.Parse<DataType>(server.DbType), server.ConnStr)
                                          .UseMonitorCommand(cmd => cmd.Executing(),
                                                             (cmd, log) => cmd.Executed(log, commandInfo))
                                          .UseAutoSyncStructure(server.AutoSyncStructure)
                                          .Build();
        // 过滤器
        {
            var user = App.GetService<IContextUser>();

            //软删除过滤器
            freeSql.GlobalFilter.ApplyOnly<IEntityDelete>(nameof(Const.Enums.FreeSqlGlobalFilters.Delete), a => a.IsDeleted ==
                                                              false);

            //租户过滤器


                if (server.MultiTenant)
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
                                                         user.DataPermission.OrgIds
                                                             .Contains(a.OwnerOrgId.Value));



        }


        me.AddSingleton(freeSql);


        me.AddScoped<UnitOfWorkManager>();
        me.AddFreeRepository(null, App.Assemblies.ToArray());
        // 事务拦截器
        me.AddScoped<TransactionInterceptor>();
        return me;
    }
}