using SimpleAdmin.WebApi.DataContracts.DbMaps;

namespace SimpleAdmin.WebApi.Repositories;

public class OperationLogRepository : RepositoryBase<TbSysOperationLog>
{
    /// <inheritdoc />
    public OperationLogRepository(IFreeSql fsql) : base(fsql)
    { }
}