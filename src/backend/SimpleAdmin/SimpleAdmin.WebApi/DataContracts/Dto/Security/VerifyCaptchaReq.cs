using SimpleAdmin.WebApi.Aop.Attributes;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     人机校验请求
/// </summary>
public record VerifyCaptchaReq : ICacheKey
{
    /// <inheritdoc cref="ICacheKey.CacheKey" />
    [RequiredField]
    public string CacheKey { get; set; }

    /// <summary>
    ///     验证数据
    /// </summary>
    [RequiredField]
    public string VerifyData { get; set; }
}