using FreeSql.DataAnnotations;
using SimpleAdmin.DataContract.DbMaps.Dependency;

namespace SimpleAdmin.DataContract.DbMaps;

[Table(Name = nameof(TbSysUser))]
[Index("idx_{tablename}_01", nameof(UserName) + "," + nameof(TenantId), true)]
public record TbSysUser : EntityTenant
{
    public string UserName { get; init; }
}