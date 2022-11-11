using System.ComponentModel.DataAnnotations;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     创建用户请求
/// </summary>
public record CreateReq : AccountInfo
{
    /// <summary>
    ///     密码
    /// </summary>
    [Required(ErrorMessage = "密码为必填项")]
    [RegularExpression("""^(?![0-9]+$)(?![a-zA-Z]+$).{8,20}$""", ErrorMessage = "密码要求8位以上数字字母组合")]
    public string Password { get; set; }

    /// <inheritdoc />
    [Required(ErrorMessage = "用户名为必填项")]
    [RegularExpression("""[a-zA-Z0-9_]{5,20}""")]
    public override string UserName { get; set; }
}