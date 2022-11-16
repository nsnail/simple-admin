using System.ComponentModel;
using Furion.FriendlyException;

namespace SimpleAdmin.WebApi.Infrastructure.Constants;

/// <summary>
///     枚举类
/// </summary>
public static class Enums
{
    /// <summary>
    ///     数据范围
    /// </summary>
    public enum DataScopes
    {
        /// <summary>
        ///     全部
        /// </summary>
        All = 1,

        /// <summary>
        ///     本部门和下级部门
        /// </summary>
        DeptWithChild = 2,

        /// <summary>
        ///     本部门
        /// </summary>
        Dept = 3,

        /// <summary>
        ///     本人数据
        /// </summary>
        Self = 4,

        /// <summary>
        ///     指定部门
        /// </summary>
        Custom = 5
    }

    /// <summary>
    ///     错误码（0 表示 成功）
    /// </summary>
    [ErrorCodeType]
    public enum ErrorCodes
    {
        /// <summary>
        ///     未知错误
        /// </summary>
        [ErrorCodeItemMetadata("{0}")] [Description("未知错误")]
        Unknown = 4000,

        /// <summary>
        ///     无效输入
        /// </summary>
        [ErrorCodeItemMetadata("{0}")] [Description("无效输入")]
        InvalidInput = 4010,

        /// <summary>
        ///     无效操作
        /// </summary>
        [ErrorCodeItemMetadata("{0}")] [Description("无效操作")]
        InvalidOperation = 4020,

        /// <summary>
        ///     未登录
        /// </summary>
        [ErrorCodeItemMetadata("{0}")] [Description("未登录")]
        IdentityMissing = 4030,

        /// <summary>
        ///     权限不足
        /// </summary>
        [ErrorCodeItemMetadata("{0}")] [Description("权限不足")]
        NoPermissions = 4031,

        /// <summary>
        ///     人机验证
        /// </summary>
        [ErrorCodeItemMetadata("需进行人机验证")] [Description("人机验证")]
        HumanVerification = 4040
    }


    /// <summary>
    ///     FreeSql全局过滤器
    /// </summary>
    public enum FreeSqlGlobalFilters
    {
        /// <summary>
        ///     数据权限
        /// </summary>
        [Description("数据权限")] Delete,

        /// <summary>
        ///     删除
        /// </summary>
        [Description("删除")] Self,

        /// <summary>
        ///     本人权限
        /// </summary>
        [Description("本人权限")] Tenant,

        /// <summary>
        ///     租户
        /// </summary>
        [Description("租户")] Data
    }

    /// <summary>
    ///     操作者
    /// </summary>
    public enum Operators
    {
        /// <summary>
        ///     用户
        /// </summary>
        User = 1,

        /// <summary>
        ///     管理员
        /// </summary>
        Administrator = 2,

        /// <summary>
        ///     服务程序
        /// </summary>
        Service = 3
    }

    /// <summary>
    ///     短信验证码类型
    /// </summary>
    public enum SmsCodeTypes
    {
        /// <summary>
        ///     注册账号
        /// </summary>
        CreateUser = 1,

        /// <summary>
        ///     登录
        /// </summary>
        Login = 2
    }

    /// <summary>
    ///     Sql命令类型
    /// </summary>
    public enum SqlCommandTypes
    {
        /// <summary>
        ///     Select
        /// </summary>
        Select,

        /// <summary>
        ///     Insert
        /// </summary>
        Insert,

        /// <summary>
        ///     Update
        /// </summary>
        Update,

        /// <summary>
        ///     Delete
        /// </summary>
        Delete
    }


    /// <summary>
    ///     用户类型
    /// </summary>
    public enum UserTypes
    {
        /// <summary>
        ///     默认用户
        /// </summary>
        DefaultUser = 1,

        /// <summary>
        ///     租户管理员
        /// </summary>
        TenantAdmin = 10,

        /// <summary>
        ///     平台管理员
        /// </summary>
        PlatformAdmin = 100
    }
}