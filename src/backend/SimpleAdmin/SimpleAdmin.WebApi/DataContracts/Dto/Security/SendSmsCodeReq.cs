using System.ComponentModel.DataAnnotations;
using SimpleAdmin.WebApi.Infrastructure.Constants;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Security;

public record SendSmsCodeReq : VerifyCaptchaReq
{
    [Required]
    [RegularExpression(Const.Templates.REGEX_MOBILE)]
    public string Mobile { get; set; }
}
