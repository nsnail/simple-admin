namespace SimpleAdmin.WebApi.DataContracts.Dto;

/// <summary>
///     RESTful 风格结果集
/// </summary>
/// <typeparam name="T"></typeparam>
public record RestfulInfo<T> : DataContract
{
    /// <summary>
    ///     代码
    /// </summary>
    public Enums.ErrorCodes Code { get; set; }

    /// <summary>
    ///     数据
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    ///     消息
    /// </summary>
    public object Msg { get; set; }
}