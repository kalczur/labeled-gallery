using LabeledGallery.Dto.User;
using LabeledGallery.Models.User;
using LabeledGallery.Utils;
using Raven.Client.Documents;

namespace LabeledGallery.Services;

public class UserService : IUserService
{
    private readonly DocumentStoreHolder _storeHolder;

    public UserService(DocumentStoreHolder storeHolder)
    {
        _storeHolder = storeHolder;
    }

    public async Task Register(RegisterRequestDto dto)
    {
        var passwordHash = HashUtils.GetStringSha256Hash(dto.Password);
        var accountLogin = new AccountLogin
        {
            Email = dto.Email,
            Password = passwordHash
        };

        var account = new Account
        {
            Email = dto.Email,
            Name = dto.Name,
            Options = new AccountOptions
            {
                ObjectsDetectionProvider = dto.ObjectsDetectionProvider!.Value
            }
        };

        using var session = _storeHolder.OpenAsyncSession();

        await session.StoreAsync(accountLogin);
        await session.StoreAsync(account);
        await session.SaveChangesAsync();
    }

    public async Task<AccountLogin?> Login(LoginRequestDto dto)
    {
        var passwordHash = HashUtils.GetStringSha256Hash(dto.Password);

        using var session = _storeHolder.OpenAsyncSession();

        return await session.Query<AccountLogin>()
            .SingleOrDefaultAsync(x => x.Email == dto.Email && x.Password == passwordHash);
    }

    public async Task<Account> GetAccountForAccountLogin(AccountLogin accountLogin)
    {
        using var session = _storeHolder.OpenAsyncSession();

        return await session.Query<Account>().SingleAsync(x => x.Email == accountLogin.Email);
    }
}
