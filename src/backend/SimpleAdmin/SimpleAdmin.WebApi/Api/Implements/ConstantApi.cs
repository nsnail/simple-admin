using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSExt.Extensions;
using SimpleAdmin.WebApi.DataContracts.Dto;
using SimpleAdmin.WebApi.Infrastructure.Constant;

namespace SimpleAdmin.WebApi.Api.Implements;

/// <inheritdoc cref="SimpleAdmin.WebApi.Api.IConstantApi" />
[AllowAnonymous]
public class ConstantApi : ApiBase<IConstantApi>, IConstantApi
{
    /// <inheritdoc />
    public object GetEnums()
    {
        return typeof(Enums).GetNestedTypes()
                            .ToDictionary(x => x.Name,
                                          x => x.GetEnumValues()
                                                .Cast<object>()
                                                .ToDictionary(x.GetEnumName,
                                                              y => new {
                                                                  Value = y,
                                                                  Desc  = ((Enum)y).Desc()
                                                              }));
    }

    /// <inheritdoc />
    [NonUnify]
    public IActionResult GetStrings()
    {
        return new JsonResult(new RestfulInfo<object> {
                                  Code = 0,
                                  Data = typeof(Strings).GetFields()
                                                        .ToDictionary(x => x.Name.ToString(),
                                                                      x => x.GetValue(null)?.ToString())
                              },
                              new JsonSerializerSettings {
                                  ContractResolver = new DefaultContractResolver()
                              });
    }
}