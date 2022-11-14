using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;

namespace SimpleAdmin.WebApi.Api.Implements;

public class TestApi : ITestApi, IDynamicApiController
{
    /// <inheritdoc />
    [AllowAnonymous]
    public string Test()
    {
        return nameof(ITestApi);
    }
}
