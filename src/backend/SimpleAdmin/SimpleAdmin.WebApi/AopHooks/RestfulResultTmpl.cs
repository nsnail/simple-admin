namespace SimpleAdmin.WebApi.AopHooks;

/// <summary>
///     RESTful 风格结果集
/// </summary>
/// <typeparam name="T"></typeparam>
public class RestfulResultTmpl<T>
{
    /// <summary>
    ///     代码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    ///     数据
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    ///     消息
    /// </summary>
    public string Message { get; set; }
}