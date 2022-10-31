namespace SimpleAdmin.Core.Constant;

public static partial class Const
{
    /// <summary>
    ///     一些模板常量
    /// </summary>
    public static class Templates
    {
        /// <summary>
        ///     日志输出模板
        /// </summary>
        public const string LOG_OUTPUT_TEMPLATE_FULL =
            "[{Timestamp:HH:mm:ss.fff} {Level:u3} {SourceContext,64}] {Message:lj}{NewLine}{Exception}";


        /// <summary>
        ///     36进制码表
        /// </summary>
        public const string SYS36_TABLE = "0123456789abcdefghijklmnopqrstuvwxyz";


        /// <summary>
        ///     手机UserAgent
        /// </summary>
        public const string UA_MOBILE =
            "Mozilla/5.0 (Linux; Android 9; Redmi Note 8 Pro Build/PPR1.180610.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/78.0.3904.96 Mobile Safari/537.36";

        /// <summary>
        ///     电脑UserAgent
        /// </summary>
        public const string UA_PC =
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";

        /// <summary>
        ///     yyyy-MM-dd
        /// </summary>
        public const string YYYY_MM_DD = "yyyy-MM-dd";

        /// <summary>
        ///     yyyy-MM-dd HH:mm:ss
        /// </summary>
        public const string YYYY_MM_DD_HH_MM_SS = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        ///     yyyyMMdd
        /// </summary>
        public const string YYYYMMDD = "yyyyMMdd";


        /// <summary>
        ///     20230204182131000+0800
        /// </summary>
        public const string YYYYMMDDHHMMSSFFFZZZZ = "yyyyMMddHHmmssfffzzz";
    }
}



