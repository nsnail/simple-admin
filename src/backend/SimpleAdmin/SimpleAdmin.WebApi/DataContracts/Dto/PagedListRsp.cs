namespace SimpleAdmin.WebApi.DataContracts.Dto;

/// <summary>
///     分页列表响应
/// </summary>
/// <typeparam name="T"></typeparam>
public record PagedListRsp<T> : DataContract where T : DataContract, new()
{
    /// <summary>
    ///     当前页码
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    ///     页容量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    ///     数据行
    /// </summary>
    public IEnumerable<T> Rows { get; set; }

    /// <summary>
    ///     数据总条
    /// </summary>
    public long Total { get; set; }
}