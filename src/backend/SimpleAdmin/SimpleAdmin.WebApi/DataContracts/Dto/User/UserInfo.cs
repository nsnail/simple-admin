namespace SimpleAdmin.WebApi.DataContracts.Dto.User;

public record UserInfo : DataContract
{
    public string UserName { get; set; }
}