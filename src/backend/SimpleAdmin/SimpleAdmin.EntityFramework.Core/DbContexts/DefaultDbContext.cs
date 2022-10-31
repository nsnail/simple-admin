using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace SimpleAdmin.EntityFramework.Core;

[AppDbContext("SimpleAdmin", DbProvider.Sqlite)]
public class DefaultDbContext : AppDbContext<DefaultDbContext>
{
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
    {
    }
}
