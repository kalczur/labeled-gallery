using LabeledGallery.Dto.User;

namespace LabeledGallery.Services;

public interface IUserService
{
    Task Register(RegisterRequestDto dto);
}