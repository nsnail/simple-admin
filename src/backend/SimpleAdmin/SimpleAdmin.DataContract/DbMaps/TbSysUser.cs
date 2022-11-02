using SimpleAdmin.DataContract.DbMaps.Dependency;

namespace SimpleAdmin.DataContract.DbMaps;

public record TbSysUser : EntityBase
{
    public string UserName { get; init; }
}