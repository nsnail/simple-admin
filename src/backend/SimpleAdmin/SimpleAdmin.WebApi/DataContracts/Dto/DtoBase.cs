using NSExt.Extensions;

namespace SimpleAdmin.WebApi.DataContracts.Dto;

/// <summary>
///     Dto 基类
/// </summary>
public abstract record DtoBase
{
    /// <inheritdoc />
    public override string ToString()
    {
        return this.Json();
    }
}