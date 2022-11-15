namespace SimpleAdmin.WebApi.Infrastructure.Utils;

public class SmsSender : ISmsSender, IScoped
{
    /// <inheritdoc />
    public void Send(string mobile, string content)
    {
        throw new NotImplementedException();
    }


    /// <inheritdoc />
    public void SendCode(string mobile, string code)
    { }
}