using Furion;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleAdmin.EntityFramework.Core;

public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDatabaseAccessor(options =>
        {
            options.AddDbPool<DefaultDbContext>();
        }, "SimpleAdmin.Database.Migrations");
    }
}
