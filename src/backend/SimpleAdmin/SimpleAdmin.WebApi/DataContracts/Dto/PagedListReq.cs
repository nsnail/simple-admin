using FreeSql.Internal.Model;

namespace SimpleAdmin.WebApi.DataContracts.Dto;

/// <summary>
///     分页列表请求
/// </summary>
/// <typeparam name="T"></typeparam>
public record PagedListReq<T> : DataContract
{
    /// <summary>
    ///     动态查询条件
    /// </summary>
    public DynamicFilterInfo DynamicFilter { get; set; } = null;

    /// <summary>
    ///     查询条件
    /// </summary>
    public T Filter { get; set; }

    /// <summary>
    ///     当前页码
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    ///     页容量
    /// </summary>
    public int PageSize { get; set; }
}