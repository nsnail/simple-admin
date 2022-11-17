namespace SimpleAdmin.WebApi.DataContracts.DbMaps.Dependency;

/// <summary>
///     比特位字段接口
/// </summary>
public interface IFieldBitSet
{
    /// <summary>
    ///     比特位
    /// </summary>
    long BitSet { get; set; }
}