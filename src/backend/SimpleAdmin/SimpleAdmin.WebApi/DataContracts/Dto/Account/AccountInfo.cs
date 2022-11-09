namespace SimpleAdmin.WebApi.DataContracts.Dto.Account;

/// <summary>
///     帐号信息
/// </summary>
public record AccountInfo : DtoBase
{
    public virtual string UserName { get; set; }
}