using LabeledGallery.Models.User;
using LabeledGallery.Utils;
using Raven.Client.Documents;

namespace LabeledGallery.Services;

public class UserService : IUserService
{
    private DocumentStoreHolder _storeHolder;
    
    public UserService(DocumentStoreHolder storeHolder)
    {
        _storeHolder = storeHolder;
    }
    
    public async Task<bool> IsEmailAlreadyExist(string email)
    {
        using (var session = _storeHolder.OpenAsyncSession())
        {
            var accountLogin = await session
                .Query<AccountLogin>()
                .SingleOrDefaultAsync(x => x.Email == email);

            return accountLogin != null;
        }
    }

    public async Task CreateAccountLogin(AccountLogin accountLogin)
    {
        using (var session = _storeHolder.OpenAsyncSession())
        {
            await session.StoreAsync(accountLogin);
            await session.SaveChangesAsync();
        }
    }

    public async Task CreateAccount(Account account)
    {
        using (var session = _storeHolder.OpenAsyncSession())
        {
            await session.StoreAsync(account);
            await session.SaveChangesAsync();
        }
    }
}