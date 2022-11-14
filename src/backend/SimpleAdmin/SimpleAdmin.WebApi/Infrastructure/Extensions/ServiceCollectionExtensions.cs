using System.Reflection;
using FreeSql;
using Furion.ConfigurableOptions;
using NSExt.Extensions;
using SimpleAdmin.WebApi.Aop.Filters;
using SimpleAdmin.WebApi.Infrastructure.Configuration.Options;
using SimpleAdmin.WebApi.Infrastructure.Utils;
using Yitter.IdGenerator;

namespace SimpleAdmin.WebApi.Infrastructure.Extensions;

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
    /// <returns></returns>
    public static IServiceCollection AddFreeSql(this IServiceCollection me)
    {
        var options = App.GetConfig<DatabaseOptions>(nameof(DatabaseOptions).TrimEndOptions());
        var freeSql = FreeSqlHelper.Create(options);
        me.AddSingleton(freeSql);
        me.AddScoped<UnitOfWorkManager>();
        me.AddFreeRepository(null, App.Assemblies.ToArray());
        // 事务拦截器
        me.AddScoped<TransactionHandler>();
        return me;
    }


    /// <summary>
    ///     注册redis缓存
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static IServiceCollection AddRedis(this IServiceCollection me)
    {
        var options = App.GetConfig<RedisOptions>(nameof(RedisOptions).TrimEndOptions());
        me.AddStackExchangeRedisCache(redisCacheOptions => {
                                          // 连接字符串，这里也可以读取配置文件
                                          redisCacheOptions.Configuration = options.ConnStr;
                                          // 键名前缀
                                          redisCacheOptions.InstanceName = $"{nameof(SimpleAdmin).Snakecase()}_";
                                      });
        return me;
    }


    /// <summary>
    ///     注册雪花id生成器
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static IServiceCollection AddSnowflake(this IServiceCollection me)
    {
        //雪花漂移算法
        var idGeneratorOptions = new IdGeneratorOptions(1) {
            WorkerIdBitLength = 6
        };
        YitIdHelper.SetIdGenerator(idGeneratorOptions);
        return me;
    }
}