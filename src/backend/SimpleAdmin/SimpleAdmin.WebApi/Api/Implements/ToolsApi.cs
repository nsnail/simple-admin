using Microsoft.AspNetCore.Authorization;

namespace SimpleAdmin.WebApi.Api.Implements;

/// <inheritdoc cref="SimpleAdmin.WebApi.Api.IToolsApi" />
[AllowAnonymous]
public class ToolsApi : ApiBase<IToolsApi>, IToolsApi
{
    /// <inheritdoc />
    public DateTime GetServerUtcTime()
    {
        return DateTime.UtcNow;
    }

    /// <inheritdoc />
    public string GetVersion()
    {
        return GetType().Assembly.GetName().Version?.ToString();
    }
}