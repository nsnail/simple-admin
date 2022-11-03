using FreeSql;
using SimpleAdmin.DataContract.DbMaps;
using SimpleAdmin.Infrastructure.Repositories;

namespace SimpleAdmin.Application.Repositories.Auth;

public class AccountRepository : RepositoryBase<TbSysUser, long>
{
    /// <inheritdoc />
    public AccountRepository(UnitOfWorkManager uowManager) : base(uowManager.Orm, uowManager)
    { }
}