using System.ComponentModel.DataAnnotations;
using SimpleAdmin.WebApi.Infrastructure.Constants;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     短信验证码信息
/// </summary>
public record SmsCodeInfo
{
    /// <summary>
    ///     验证码
    /// </summary>
    [RegularExpression(Strings.REGEX_SMSCODE, ErrorMessage = Strings.RULE_SMSCODE)]
    public virtual string Code { get; set; }


    /// <summary>
    ///     创建时间
    /// </summary>
    public virtual DateTime CreateTime { get; set; }


    /// <summary>
    ///     手机号
    /// </summary>
    [RegularExpression(Strings.REGEX_MOBILE, ErrorMessage = Strings.RULE_MOBILE)]
    public virtual string Mobile { get; set; }


    /// <summary>
    ///     类型
    /// </summary>
    public virtual Enums.SmsCodeTypes Type { get; set; }
}