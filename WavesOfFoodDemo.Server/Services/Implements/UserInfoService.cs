using AutoMapper;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;
using WavesOfFoodDemo.Server.Infrastructures;

namespace WavesOfFoodDemo.Server.Services;

public class UserInfoService : IUserInfoService
{
    private readonly ILogger<UserInfoService> _logger;
    private readonly IUserInfoRepository _userInfoRepository;
    private readonly IMapper _mapper;

    public UserInfoService(IUserInfoRepository userInfoRepository, ILogger<UserInfoService> logger, IMapper mapper)
    {
        _userInfoRepository = userInfoRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<UserInfoDto>> GetUserInfoDtosAsync()
    {
        try
        {
            var data = await _userInfoRepository.GetAllAsync();
            return _mapper.Map<List<UserInfoDto>>(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<bool> AddUserInfoAsync(UserInfoCreateDto userInfoCreateDto)
    {
        try
        {
            var info = _mapper.Map<UserInfo>(userInfoCreateDto);
            return await _userInfoRepository.AddAsync(info);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<bool?> EditUserInfoAsync(UserInfoDto userInfoDto)
    {
        try
        {
            var userInfo = await _userInfoRepository.GetByIdAsync(userInfoDto.Id);
            if (userInfo == null)
            {
                return null;
            }
            var infoUpdate = _mapper.Map<UserInfo>(userInfoDto);
            var result = await _userInfoRepository.UpdateAsync(infoUpdate);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<bool?> RemoveUserInfoDtosAsync(Guid id)
    {
        try
        {
            return await _userInfoRepository.DeleteByKey(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
    public async Task<List<UserInfoDto>> SearchUserInfoDtosAsync(string userName)
    {
        try
        {
            var data = await _userInfoRepository.SearchUserInfoDtosAsync(userName);
            return _mapper.Map<List<UserInfoDto>>(data);
        }
        catch ( Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<UserInfoDto> LoginUserInfoAsync(string userName, string userPassword)
    {
        try
        {
            var data = await _userInfoRepository.LoginUserInfoAsync(userName, userPassword);
            if (data != null)
            {
                return _mapper.Map<UserInfoDto>(data);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}