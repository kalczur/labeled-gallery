using LabeledGallery.Dto.User;
using LabeledGallery.Models.User;
using LabeledGallery.Services;
using LabeledGallery.Utils.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LabeledGallery.Controllers;

[ApiController]
[Route("/api/v1/user/")]
public class UserController : AbstractController
{
    private readonly ISignInManager _signInManager;
    private readonly UserManager<AccountLogin> _userManager;
    private readonly IUserService _userService;

    public UserController(IUserService userService, ISignInManager signInManager, UserManager<AccountLogin> userManager)
    {
        _userService = userService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        await _userService.Register(dto);
        return Ok();
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        var accountLogin = await _userService.Login(dto);

        if (accountLogin == null)
            return BadRequest();

        await _signInManager.SignInAsync(accountLogin);
        return Ok();
    }

    [Authorize]
    [Route("logout")]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [Route("info")]
    [HttpGet]
    public async Task<IActionResult> GetUserInfo()
    {
        var identity = HttpContext.User.Identity;

        if (identity?.IsAuthenticated == false)
            return Ok(new UserInfoDto { IsAuthenticated = false });

        var accountLogin = await GetCurrentAccountLogin(_userManager);
        var accountResults = await _userService.GetAccountForAccountLogin(accountLogin);

        var accountDto = AccountDto.FromModel(accountResults);

        return Ok(new UserInfoDto
        {
            IsAuthenticated = true,
            Account = accountDto
        });
    }
}
