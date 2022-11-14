namespace SimpleAdmin.WebApi.Infrastructure.Configuration.Options;

/// <summary>
///     Redis配置
/// </summary>
public record RedisOptions : OptionAbstraction
{
    /// <summary>
    ///     链接字符串
    /// </summary>
    public string ConnStr { get; set; }
}