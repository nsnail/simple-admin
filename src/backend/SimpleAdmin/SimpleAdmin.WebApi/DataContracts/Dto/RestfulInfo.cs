using Newtonsoft.Json;
using SimpleAdmin.WebApi.Infrastructure.Constant;

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
    public Enums.ErrorCodes Code { get; set; }

    /// <summary>
    ///     数据
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; }

    /// <summary>
    ///     消息
    /// </summary>
    [JsonProperty("msg")]
    public object Msg { get; set; }
}