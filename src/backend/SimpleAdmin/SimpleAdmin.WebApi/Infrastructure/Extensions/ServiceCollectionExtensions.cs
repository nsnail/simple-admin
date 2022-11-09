using System.Reflection;
using Furion.ConfigurableOptions;
using SimpleAdmin.WebApi.Infrastructure.Constants;

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
    /// <param name="name"></param>
    /// <param name="commandInfo"></param>
    /// <returns></returns>
    public static IServiceCollection AddFreeSql(this IServiceCollection                             me,
                                                string                                              name,
                                                Action<Const.Enums.SqlCommandTypes, string, string> commandInfo = null)
    {
        return me;
    }
}