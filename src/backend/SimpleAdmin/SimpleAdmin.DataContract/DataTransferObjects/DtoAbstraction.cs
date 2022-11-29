using NSExt.Extensions;

namespace SimpleAdmin.DataContract.DataTransferObjects;

/// <summary>
///     Dto 抽象基类
/// </summary>
public record DtoAbstraction
{
    /// <inheritdoc />
    public override string ToString()
    {
        return this.Json();
    }
}