using LabeledGallery.Models.User;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace LabeledGallery.Utils.Auth;

public class AuthUserRepository : IAuthUserRepository
{
    private readonly ILogger<AuthUserRepository> _logger;
    private readonly IAsyncDocumentSession _session;

    public AuthUserRepository(IAsyncDocumentSession session, ILogger<AuthUserRepository> logger)
    {
        _session = session;
        _logger = logger;
    }

    public async Task<AccountLogin> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _session.LoadAsync<AccountLogin>(userId, cancellationToken);
    }

    public async Task<AccountLogin> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await _session.Query<AccountLogin>()
            .Where(x => x.Email == normalizedUserName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AccountLogin> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return await _session.Query<AccountLogin>()
            .Where(x => x.Email == normalizedEmail)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IdentityResult> RegisterAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        try
        {
            await _session.StoreAsync(accountLogin, cancellationToken);
            await _session.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed to register customer {accountLogin.Email}");
            return IdentityResult.Failed(new IdentityError { Description = e.Message });
        }
    }

    public async Task<IdentityResult> UpdateAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        try
        {
            await _session.StoreAsync(accountLogin);
            await _session.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed to update customer {accountLogin.Email}");
            return IdentityResult.Failed(new IdentityError { Description = e.Message });
        }
    }

    public async Task<IdentityResult> DeleteAsync(AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        try
        {
            _session.Delete(accountLogin);
            await _session.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed to delete customer {accountLogin.Email}");
            return IdentityResult.Failed(new IdentityError { Description = e.Message });
        }
    }
}
