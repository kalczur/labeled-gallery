using LabeledGallery.Models.User;

namespace LabeledGallery.Services;

public interface IUserService
{
    Task<bool> IsEmailAlreadyExist(string email);
    Task CreateAccountLogin(AccountLogin accountLogin);
    Task CreateAccount(Account account);
}