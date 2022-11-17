using SimpleAdmin.WebApi.DataContracts.Dto.User;

namespace SimpleAdmin.WebApi.Api;

/// <summary>
///     用户接口
/// </summary>
public interface IUserApi
{
    /// <summary>
    ///     获取个人信息
    /// </summary>
    /// <returns></returns>
    Task<ProfileRsp> GetProfile();
}