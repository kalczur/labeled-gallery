using LabeledGallery.Dto.User;
using LabeledGallery.Models.Gallery;
using LabeledGallery.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace LabeledGallery.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    [Route("/api/v1/user/register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        // TODO - implement
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