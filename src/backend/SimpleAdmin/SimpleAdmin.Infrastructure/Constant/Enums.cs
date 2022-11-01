namespace SimpleAdmin.Infrastructure.Constant;

public static partial class Const
{
    /// <summary>
    ///     枚举类
    /// </summary>
    public static class Enums
    {
        /// <summary>
        ///     操作者
        /// </summary>
        public enum Operators : byte
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
    }
}



