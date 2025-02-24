using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public interface IUserInfoRepository : IGenericRepository<UserInfo>
{
    Task<List<UserInfo>> SearchUserInfoDtosAsync(string userName);

    Task<UserInfo> LoginUserInfoAsync(string userName, string userPassword);
}
