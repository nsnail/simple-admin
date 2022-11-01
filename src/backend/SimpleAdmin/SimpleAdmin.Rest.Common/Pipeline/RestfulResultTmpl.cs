using SimpleAdmin.Infrastructure.Constant;

namespace SimpleAdmin.Rest.Common.Pipeline;

/// <summary>
///     RESTful 风格结果集
/// </summary>
/// <typeparam name="T"></typeparam>
public class RestfulResultTmpl<T>
{
    /// <summary>
    ///     数据
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    ///     错误码
    /// </summary>
    public ErrorCodes ErrorCode { get; set; }

    /// <summary>
    ///     错误信息
    /// </summary>
    public object Errors { get; set; }

    /// <summary>
    ///     附加数据
    /// </summary>
    public object Extras { get; set; }

    /// <summary>
    ///     执行成功
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    ///     时间戳
    /// </summary>
    public long Timestamp { get; set; }
}




