using LabeledGallery.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LabeledGallery.Controllers;

public abstract class AbstractController : ControllerBase
{
    protected string AccountEmail => HttpContext.User.Identity?.Name;

    protected Task<AccountLogin> GetCurrentAccountLogin(UserManager<AccountLogin> userManager)
    {
        var identity = HttpContext.User.Identity;
        return userManager.FindByEmailAsync(identity?.Name);
    }
}
