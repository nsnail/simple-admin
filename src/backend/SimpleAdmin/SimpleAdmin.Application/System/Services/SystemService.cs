using Microsoft.Extensions.Logging;

namespace SimpleAdmin.Application.System.Services;

public class SystemService : ISystemService, ITransient
{
    private readonly ILogger<SystemService> _logger;

    public SystemService(ILogger<SystemService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     开发更简单，更通用，更流行。
    /// </summary>
    /// <returns></returns>
    public string GetDescription()
    {
        _logger.LogInformation("sdfsdf");
        return "让 .NET 开发更简单，更通用，更流行。";
    }
}



