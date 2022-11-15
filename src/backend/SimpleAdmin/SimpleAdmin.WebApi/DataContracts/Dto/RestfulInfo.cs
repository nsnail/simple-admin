using Newtonsoft.Json;

namespace SimpleAdmin.WebApi.DataContracts.Dto;

/// <summary>
///     RESTful 风格结果集
/// </summary>
/// <typeparam name="T"></typeparam>
public record RestfulInfo<T> : DtoBase
{
    /// <summary>
    ///     代码
    /// </summary>
    [JsonProperty("code")]
    public object Code { get; set; }

    /// <summary>
    ///     数据
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; }

    /// <summary>
    ///     消息
    /// </summary>
    [JsonProperty("message")]
    public object Message { get; set; }
}