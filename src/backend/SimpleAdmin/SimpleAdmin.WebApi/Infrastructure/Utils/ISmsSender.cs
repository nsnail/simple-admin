namespace SimpleAdmin.WebApi.Infrastructure.Utils;

public interface ISmsSender
{
    void Send(string mobile, string content);

    void SendCode(string mobile, string code);
}