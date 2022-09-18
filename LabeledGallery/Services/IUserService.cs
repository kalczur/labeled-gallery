using LabeledGallery.Dto.User;
using LabeledGallery.Models.User;

namespace LabeledGallery.Services;

public interface IUserService
{
    Task Register(RegisterRequestDto dto);
    Task<AccountLogin?> Login(LoginRequestDto dto);
    Task<Account> GetAccountForAccountLogin(AccountLogin accountLogin);
}
