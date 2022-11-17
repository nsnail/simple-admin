namespace SimpleAdmin.WebApi.DataContracts.Dto.User;

public record UserInfo : DtoBase
{
    public string UserName { get; set; }
}