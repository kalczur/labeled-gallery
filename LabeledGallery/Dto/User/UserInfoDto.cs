using LabeledGallery.Models.User;

namespace LabeledGallery.Dto.User;

public class UserInfoDto
{
    public bool IsAuthenticated { get; set; }
    public Account Account { get; set; }
}