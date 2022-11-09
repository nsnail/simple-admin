using SimpleAdmin.WebApi.DataContracts.DbMaps;

namespace SimpleAdmin.WebApi.Repositories;

public class AccountRepository : RepositoryBase<TbSysUser>
{
    /// <inheritdoc />
    public AccountRepository(IFreeSql fsql) : base(fsql)
    { }
}