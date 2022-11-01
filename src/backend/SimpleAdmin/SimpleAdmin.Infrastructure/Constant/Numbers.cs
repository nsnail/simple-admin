using System;

namespace SimpleAdmin.Infrastructure.Constant;

public static partial class Const
{
    /// <summary>
    ///     一些值类型常量
    /// </summary>
    public static class Values
    {
        /// <summary>
        ///     预设的cookie最大日起值
        /// </summary>
        public static readonly DateTime COOKIE_MAX_DATE = DateTime.Now.AddYears(100);

        /// <summary>
        ///     最大页码
        /// </summary>
        public const int PAGE_NO_MAX = 10000;

        /// <summary>
        ///     最小页码
        /// </summary>
        public const int PAGE_NO_MIN = 1;

        /// <summary>
        ///     最大分页容量
        /// </summary>
        public const int PAGE_SIZE_MAX = 100;

        /// <summary>
        ///     最小分页容量
        /// </summary>
        public const int PAGE_SIZE_MIN = 1;

        /// <summary>
        ///     sql server中的最小日期值
        /// </summary>
        public static readonly DateTime SQL_MIN_DATE = new(1753, 1, 1, 12, 0, 0);
    }
}



