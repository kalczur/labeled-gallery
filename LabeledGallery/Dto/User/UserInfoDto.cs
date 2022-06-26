namespace LabeledGallery.Dto.User;

public class UserInfoDto
{
    public bool IsAuthenticated { get; set; }
    public AccountDto Account { get; set; }
}