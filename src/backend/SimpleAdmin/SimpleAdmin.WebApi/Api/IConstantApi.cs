using Microsoft.AspNetCore.Mvc;

namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     常量接口
/// </summary>
public interface IConstantApi
{
    /// <summary>
    ///     获得常用消息
    /// </summary>
    /// <returns></returns>
    public IActionResult GetStrings();
}