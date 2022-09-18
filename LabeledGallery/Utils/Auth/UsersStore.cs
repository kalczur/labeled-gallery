using LabeledGallery.Models.User;
using Microsoft.AspNetCore.Identity;

namespace LabeledGallery.Utils.Auth;

public class UsersStore : IUserEmailStore<AccountLogin>
{
    private readonly IAuthUserRepository _authUserRepository;

    public UsersStore(IAuthUserRepository authUserRepository, ILogger<UsersStore> logger)
    {
        _authUserRepository = authUserRepository;
    }

    public void Dispose()
    {
    }

    public Task<string> GetUserIdAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        return Task.FromResult(accountLogin.Id);
    }

    public Task<string> GetUserNameAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        return Task.FromResult(accountLogin.Email);
    }

    public Task SetUserNameAsync(AccountLogin accountLogin, string userName, CancellationToken cancellationToken)
    {
        accountLogin.Email = userName;
        return Task.CompletedTask;
    }

    public Task<string> GetNormalizedUserNameAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        return Task.FromResult(accountLogin.Email);
    }

    public Task SetNormalizedUserNameAsync(AccountLogin accountLogin, string normalizedName,
        CancellationToken cancellationToken)
    {
        accountLogin.Email = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> CreateAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        accountLogin.SecurityStamp = Guid.NewGuid().ToString();
        return await _authUserRepository.RegisterAsync(accountLogin, cancellationToken);
    }

    public async Task<IdentityResult> UpdateAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        return await _authUserRepository.UpdateAsync(accountLogin, cancellationToken);
    }

    public Task<IdentityResult> DeleteAsync(AccountLogin user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<AccountLogin> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _authUserRepository.FindByIdAsync(userId, cancellationToken);
    }

    public async Task<AccountLogin> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await _authUserRepository.FindByNameAsync(normalizedUserName, cancellationToken);
    }

    public Task SetEmailAsync(AccountLogin accountLogin, string email, CancellationToken cancellationToken)
    {
        accountLogin.Email = email;
        return Task.CompletedTask;
    }

    public Task<string> GetEmailAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        return Task.FromResult(accountLogin.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(AccountLogin user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailConfirmedAsync(AccountLogin user, bool confirmed, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<AccountLogin> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return await _authUserRepository.FindByEmailAsync(normalizedEmail, cancellationToken);
    }

    public Task<string> GetNormalizedEmailAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        return Task.FromResult(accountLogin.Email.ToLowerInvariant());
    }

    public Task SetNormalizedEmailAsync(AccountLogin accountLogin, string normalizedEmail,
        CancellationToken cancellationToken)
    {
        accountLogin.Email = normalizedEmail;
        return Task.CompletedTask;
    }
}
