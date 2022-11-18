namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     工具接口
/// </summary>
public interface IToolsApi
{
    /// <summary>
    ///     服务器时间
    /// </summary>
    /// <returns></returns>
    DateTime GetServerUtcTime();

    /// <summary>
    ///     获取版本号
    /// </summary>
    /// <returns></returns>
    string GetVersion();
}