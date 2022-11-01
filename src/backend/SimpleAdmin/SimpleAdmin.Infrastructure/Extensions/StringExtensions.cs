using System.Text.RegularExpressions;

namespace SimpleAdmin.Infrastructure.Extensions;

/// <summary>
///     String  扩展方法
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     去掉尾部字符串“Options”
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public static string TrimEndOptions(this string me)
    {
        return new Regex(@"Options$").Replace(me, string.Empty);
    }
}