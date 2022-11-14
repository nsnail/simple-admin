namespace SimpleAdmin.WebApi.Infrastructure.Configuration.Options;

/// <summary>
///     密码配置
/// </summary>
public record SecretOptions : OptionAbstraction
{
    /// <summary>
    ///     SecretKeyA
    /// </summary>
    public string SecretKeyA { get; set; }
}