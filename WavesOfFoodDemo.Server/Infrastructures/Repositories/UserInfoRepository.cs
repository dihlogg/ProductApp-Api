using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public class UserInfoRepository : GenericRepository<UserInfo>, IUserInfoRepository
{
    public UserInfoRepository(ProductDbContext productDbContext) : base(productDbContext)
    {
    }

    public async Task<List<UserInfo>> SearchUserInfoDtosAsync(string userName)
    {
        var query = _productDbContext.UserInfos.AsQueryable();
        query = query.Where(s => s.UserName.Contains(userName));
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<UserInfo> LoginUserInfoAsync(string userName, string userPassword)
    {
        var query = _productDbContext.UserInfos.AsQueryable();
        query = query.Where(s => s.UserName == userName && s.UserPassword == userPassword);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<UserInfoDto?> GetUserInfoByIdAsync(Guid id)
    {
        return await _productDbContext.UserInfos
            .Where(u => u.Id == id)
            .Select(u => new UserInfoDto
            {
                Id = u.Id,
                UserName = u.UserName,
                UserPassword = u.UserPassword,
                UserFullName = u.UserFullName,
                UserAddress = u.UserAddress,
                UserPhone = u.UserPhone,
            })
            .FirstOrDefaultAsync();
    }
}
