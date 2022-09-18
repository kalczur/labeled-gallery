using LabeledGallery.Models.User;

namespace LabeledGallery.Dto.User;

public class AccountDto
{
    public string Email { get; set; }
    public string Name { get; set; }
    public AccountOptions Options { get; set; }

    public static AccountDto FromModel(Account account)
    {
        return new AccountDto
        {
            Email = account.Email,
            Name = account.Name,
            Options = account.Options
        };
    }
}
