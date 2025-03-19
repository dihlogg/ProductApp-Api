using Microsoft.AspNetCore.Mvc;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Services;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly ILogger<UserInfoController> _logger;
        private readonly IUserInfoService _userInfoService;

        public UserInfoController(
            ILogger<UserInfoController> logger,
            IUserInfoService userInfoService)
        {
            _logger = logger;
            _userInfoService = userInfoService;
        }

        [HttpGet("GetUserInfos")]
        public async Task<IActionResult> GetUserInfos()
        {
            try
            {
                var data = await _userInfoService.GetUserInfoDtosAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetUserInfoByIdAsync/{id}")]
        public async Task<IActionResult> GetUserInfoByIdAsync(Guid id)
        {
            try
            {
                var data = await _userInfoService.GetUserInfoByIdAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SearchUserInfos")]
        public async Task<IActionResult> SearchUserInfos(string userName)
        {
            try
            {
                var data = await _userInfoService.SearchUserInfoDtosAsync(userName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostUserInfo")]
        public async Task<IActionResult> PostUserInfo(UserInfoCreateDto userInfoCreateDto)
        {
            try
            {
                var data = await _userInfoService.AddUserInfoAsync(userInfoCreateDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUserInfo")]
        public async Task<IActionResult> PutUserInfo(UserInfoDto userInfoDto)
        {
            try
            {
                var data = await _userInfoService.EditUserInfoAsync(userInfoDto);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUserInfo/{id}")]
        public async Task<IActionResult> DeleteUserInfo(Guid id)
        {
            try
            {
                var data = await _userInfoService.RemoveUserInfoDtosAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUserInfoAsync(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var userInfo = await _userInfoService.LoginUserInfoAsync(userLoginDto.UserName, userLoginDto.PassWord);
                if (userInfo != null)
                {
                    return Ok(userInfo);
                }
                else
                {
                    return Unauthorized("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}