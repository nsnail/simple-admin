using System.ComponentModel.DataAnnotations;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     创建用户请求
/// </summary>
public record CreateReq : AccountInfo
{
    /// <inheritdoc />
    [Required]
    [RegularExpression("""[a-zA-Z0-9_]+""")]
    [MaxLength(20)]
    public override string UserName { get; set; }
}