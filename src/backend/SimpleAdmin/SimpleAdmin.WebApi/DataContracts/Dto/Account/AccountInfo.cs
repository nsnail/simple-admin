using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NSExt.Extensions;
using SimpleAdmin.WebApi.Infrastructure.Configuration.Options;
using SimpleAdmin.WebApi.Infrastructure.Constant;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     帐号信息
/// </summary>
public record AccountInfo : DtoBase
{
    public AccountInfo()
    {
        _secretOptions = App.GetOptions<SecretOptions>();
    }

    private readonly SecretOptions _secretOptions;

    /// <summary>
    ///     密码
    /// </summary>
    public virtual string Password { get; set; }


    /// <summary>
    ///     密码原文
    /// </summary>
    [RegularExpression(Strings.REGEX_PASSWORD, ErrorMessage = Strings.MSG_PASSWORD_STRONG)]
    [JsonIgnore]
    public virtual string PasswordOrigin => Password?.AesDe(_secretOptions.SecretKeyA);

    /// <summary>
    ///     用户名
    /// </summary>
    [RegularExpression(Strings.REGEX_USERNAME, ErrorMessage = Strings.MSG_USERNAME_STRONG)]
    public virtual string UserName { get; set; }
}