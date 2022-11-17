using SimpleAdmin.WebApi.DataContracts.DbMaps;

namespace SimpleAdmin.WebApi.Repositories;

public class UserRepository : RepositoryBase<TbSysUser>
{
    /// <inheritdoc />
    public UserRepository(IFreeSql fsql) : base(fsql)
    { }
}