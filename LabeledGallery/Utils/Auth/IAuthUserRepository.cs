using LabeledGallery.Models.User;
using Microsoft.AspNetCore.Identity;

namespace LabeledGallery.Utils.Auth;

public interface IAuthUserRepository
{
    Task<AccountLogin> FindByIdAsync(string userId, CancellationToken cancellationToken);
    Task<AccountLogin> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
    Task<AccountLogin> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
    Task<IdentityResult> RegisterAsync(AccountLogin accountLogin, CancellationToken cancellationToken);
    Task<IdentityResult> UpdateAsync(AccountLogin accountLogin, CancellationToken cancellationToken);
}
