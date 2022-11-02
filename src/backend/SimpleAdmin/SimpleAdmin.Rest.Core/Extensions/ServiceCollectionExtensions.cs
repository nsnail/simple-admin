using System;
using System.Linq;
using System.Reflection;
using FreeSql;
using Furion;
using Furion.ConfigurableOptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimpleAdmin.Infrastructure.Configuration.Options;
using SimpleAdmin.Infrastructure.Constant;
using SimpleAdmin.Infrastructure.Extensions;
using SimpleAdmin.Rest.Core.Pipelines;

namespace SimpleAdmin.Rest.Core.Extensions;

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
        var server = App.GetConfig<ConnectionsOptions>(nameof(ConnectionsOptions).TrimEndOptions())[name];
        var freeSql = new FreeSqlBuilder().UseConnectionString(Enum.Parse<DataType>(server.DbType), server.ConnStr)
                                          .UseMonitorCommand(cmd => cmd.Executing(),
                                                             (cmd, log) => cmd.Executed(log, commandInfo))
                                          .UseAutoSyncStructure(server.AutoSyncStructure)
                                          .Build();
        me.AddSingleton(freeSql);


        me.AddScoped<UnitOfWorkManager>();
        me.AddFreeRepository(null, App.Assemblies.ToArray());
        // AOP事务管道
        me.AddScoped(typeof(IPipelineBehavior<,>), typeof(FreeSqlTransactionBehavior<,>));
        return me;
    }


    /// <summary>
    ///     注册 MediatR 组件
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static IServiceCollection AddMediatR(this IServiceCollection me)
    {
        me.AddMediatR(App.EffectiveTypes.ToArray());
        return me;
    }
}