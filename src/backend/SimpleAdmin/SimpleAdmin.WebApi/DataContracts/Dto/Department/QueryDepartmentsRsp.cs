using SimpleAdmin.WebApi.DataContracts.DbMaps;

namespace SimpleAdmin.WebApi.DataContracts.Dto.Department;

public record QueryDepartmentsRsp : TbSysDepartment
{
    /// <inheritdoc />
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public new List<QueryDepartmentsRsp> Children { get; set; }

    /// <inheritdoc />

    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public override string Label { get; set; }
}