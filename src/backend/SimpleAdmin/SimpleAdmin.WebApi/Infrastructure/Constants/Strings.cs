#pragma warning disable CS1591
namespace SimpleAdmin.WebApi.Infrastructure.Constants;

public static class Strings
{
    public const string FMT_YYYY_MM_DD = "yyyy-MM-dd";


    public const string FMT_YYYY_MM_DD_HH_MM_SS = "yyyy-MM-dd HH:mm:ss";


    public const string FMT_YYYYMMDD = "yyyyMMdd";


    public const string FMT_YYYYMMDDHHMMSSFFFZZZZ = "yyyyMMddHHmmssfffzzz";
    public const string REGEX_MOBILE              = """^1(3\d|4[5-9]|5[0-35-9]|6[6]|7[2-8]|8\d|9[0-35-9])\d{8}$""";


    public const string REGEX_PASSWORD = """^(?![0-9]+$)(?![a-zA-Z]+$).{8,20}$""";
    public const string REGEX_SMSCODE  = """^\d{4}$""";
    public const string REGEX_USERNAME = """^[a-zA-Z0-9_]{5,20}$""";
    public const string RULE_MOBILE    = "能正常使用的手机号码";
    public const string RULE_PASSWORD  = "8位以上数字字母组合";
    public const string RULE_REQUIRED  = "必填";
    public const string RULE_SMSCODE   = "验证码短信中的4位数字";
    public const string RULE_USERNAME  = "5位以上（字母数字下划线）";

    public const string TEMP_LOG_OUPUT =
        "[{Timestamp:HH:mm:ss.fff} {Level:u3} {SourceContext,64}] {Message:lj}{NewLine}{Exception}";

    public const string TEMP_SMSCODE = "您正在进行 {0} 操作，验证码为：{1}，5分钟内有效，如非本人操作，请忽略。";


    public const string UA_MOBILE =
        "Mozilla/5.0 (Linux; Android 9; Redmi Note 8 Pro Build/PPR1.180610.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/78.0.3904.96 Mobile Safari/537.36";


    public const string UA_PC =
        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
}