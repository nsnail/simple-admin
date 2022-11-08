using SimpleAdmin.DataContract.DbMaps;
using SimpleAdmin.Infrastructure.Configuration.Options.Nodes.Connection;

namespace SimpleAdmin.WebApi.Utils;

public class CustomSyncData : SyncData, ISyncData
{
    public virtual async Task SyncDataAsync(IFreeSql db, ServersNode server)
    {
        using var       uow  = db.CreateUnitOfWork();
        await using var tran = uow.GetOrBeginTransaction();

        var users = GetData<TbSysUser>(server.IsMultiTenant);
        await InitDataAsync(db, uow, tran, users, server);


        uow.Commit();
    }
}