using EntityBase = SimpleAdmin.DataContract.DbMaps.Dependency.EntityBase;

namespace SimpleAdmin.DataContract.DbMaps;

public record TbSysUser : EntityBase
{
    public string UserName { get; init; }
}