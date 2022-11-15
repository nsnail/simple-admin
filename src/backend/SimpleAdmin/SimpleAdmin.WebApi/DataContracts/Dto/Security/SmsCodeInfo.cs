using System.ComponentModel;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

/// <summary>
///     短信数字码信息
/// </summary>
public record SmsCodeInfo
{
    /// <summary>
    ///     数字码
    /// </summary>
    public virtual string Code { get; set; }


    /// <summary>
    ///     创建时间
    /// </summary>
    public virtual DateTime CreateTime { get; set; }


    /// <summary>
    ///     手机号
    /// </summary>
    public virtual string Mobile { get; set; }


    /// <summary>
    ///     操作类型
    /// </summary>
    public virtual OperationTypes OperationType { get; set; }
}

/// <summary>
///     操作类型
/// </summary>
public enum OperationTypes
{
    /// <summary>
    ///     注册账号
    /// </summary>
    [Description(nameof(注册帐号))] 注册帐号,

    /// <summary>
    ///     登录
    /// </summary>
    [Description(nameof(登录))] 登录
}