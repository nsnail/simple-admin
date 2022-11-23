using SimpleAdmin.WebApi.DataContracts.DbMaps;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Department;

public record QueryDepartmentsReq : TbSysDepartment
{
    /// <inheritdoc />
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public override string Label { get; set; }
}