using Microsoft.AspNetCore.Mvc;

namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     常量接口
/// </summary>
public interface IConstantApi
{
    /// <summary>
    ///     获得枚举常量
    /// </summary>
    /// <returns></returns>
    object GetEnums();

    /// <summary>
    ///     获得字符串常量
    /// </summary>
    /// <returns></returns>
    IActionResult GetStrings();
}