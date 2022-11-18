using NSExt.Extensions;
using SimpleAdmin.WebApi.Aop.Attributes;
using SimpleAdmin.WebApi.DataContracts.DbMaps;
using SimpleAdmin.WebApi.DataContracts.Dto.Security;
using SimpleAdmin.WebApi.Infrastructure.Configuration.Options;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     创建用户请求
/// </summary>
public record CreateReq : TbSysUser
{
    /// <inheritdoc />
    public CreateReq()
    {
        _secretOptions = App.GetOptions<SecretOptions>();
    }

    private readonly SecretOptions _secretOptions;

    /// <inheritdoc cref="TbSysUser.Password" />
    [RequiredField]
    public new string Password { get; set; }


    /// <summary>
    ///     密码原文
    /// </summary>
    [RegularExpression(Strings.REGEX_PASSWORD, ErrorMessage = Strings.MSG_PASSWORD_STRONG)]
    [JsonIgnore]
    public virtual string PasswordOrigin => Password?.AesDe(_secretOptions.SecretKeyA);

    /// <inheritdoc cref="TbSysUser.UserName" />
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [RequiredField]
    public override string UserName { get; set; }


    /// <summary>
    ///     短信验证码信息
    /// </summary>
    [RequiredField]
    public VerifySmsCodeReq VerifySmsCodeReq { get; set; }
}