namespace SimpleAdmin.WebApi.Infrastructure.Utils;

public interface ISmsSender
{
    void Send(long mobile, string content);

    void SendCode(long mobile, string code);
}