using SimpleAdmin.Infrastructure.Configuration.Options.Nodes.Connection;

namespace SimpleAdmin.WebApi.Utils;

/// <summary>
///     同步数据接口
/// </summary>
public interface ISyncData
{
    Task SyncDataAsync(IFreeSql db, ServersNode server);
}