using LabeledGallery.Dto.User;
using LabeledGallery.Models.Gallery;
using LabeledGallery.Models.User;
using LabeledGallery.Services;
using LabeledGallery.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LabeledGallery.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Route("/api/v1/user/register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        // TODO - add validation

        var isMailAlreadyExist = await _userService.IsEmailAlreadyExist(dto.Email);

        if (isMailAlreadyExist)
        {
            return BadRequest();
        }

        var passwordHash = HashUtils.GetStringSha256Hash(dto.Password);
        var accountLogin = new AccountLogin
        {
            Email = dto.Email,
            Password = passwordHash
        };
        
        await _userService.CreateAccountLogin(accountLogin);
        
        var account = new Account
        {
            Email = dto.Email,
            Name = dto.Name,
            Options = new AccountOptions
            {
                ObjectsDetectionProvider = dto.ObjectsDetectionProvider
            }
        };
        
        await _userService.CreateAccount(account);
        
        return Ok();
    }
    
    [Route("/api/v1/user/login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        // TODO - implement
        return Ok();
    }
    
    [Route("/api/v1/user/info")]
    [HttpGet]
    public async Task<IActionResult> GetUserInfo()
    {
        // TODO - implement
        return Ok(GetValidUserInfo());
    }

    // STUBS

    private static UserInfoDto GetValidUserInfo()
    {
        return new UserInfoDto
        {
            IsAuthenticated = true,
            Account = new AccountDto
            {
                Email = "some@mail.net",
                Name = "some-name",
                Options = new AccountOptions
                {
                    ObjectsDetectionProvider = ObjectsDetectionProvider.Gcp
                }
            }
        };
    }
}