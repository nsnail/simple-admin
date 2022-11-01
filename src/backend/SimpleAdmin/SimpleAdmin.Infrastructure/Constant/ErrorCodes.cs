using Furion.FriendlyException;

namespace SimpleAdmin.Infrastructure.Constant;

/// <summary>
///     错误码
/// </summary>
[ErrorCodeType]
public enum ErrorCodes
{
    /// <summary>
    ///     未知错误
    /// </summary>
    [ErrorCodeItemMetadata("{0}")] 未知错误 = 40000,

    /// <summary>
    ///     无效输入
    /// </summary>
    [ErrorCodeItemMetadata("{0}")] 无效输入 = 40100,

    /// <summary>
    ///     无效操作
    /// </summary>
    [ErrorCodeItemMetadata("{0}")] 无效操作 = 40200
}