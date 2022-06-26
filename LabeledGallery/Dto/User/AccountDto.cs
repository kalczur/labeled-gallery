using LabeledGallery.Models.User;

namespace LabeledGallery.Dto.User;

public class AccountDto
{
    public string Email { get; set; }
    public string Name { get; set; }
    public AccountOptions Options { get; set; }
}