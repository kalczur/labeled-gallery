using LabeledGallery.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LabeledGallery.Utils.Auth;

public interface ISignInManager
{
    Task SignInAsync(AccountLogin accountLogin);
    Task SignOutAsync();
}

public class SignInManager : SignInManager<AccountLogin>, ISignInManager
{
    public SignInManager(
        UserManager<AccountLogin> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<AccountLogin> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<AccountLogin>> logger,
        IAuthenticationSchemeProvider schemes, IUserConfirmation<AccountLogin> confirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }

    public virtual async Task SignInAsync(AccountLogin accountLogin)
    {
        if (accountLogin == null)
            throw new ArgumentNullException(nameof(accountLogin));

        await base.SignInAsync(accountLogin, new AuthenticationProperties { IsPersistent = false });
    }
}
