using LabeledGallery.Models.Gallery;

namespace LabeledGallery.Dto.User;

public class RegisterRequestDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public ObjectsDetectionProvider ObjectsDetectionProvider { get; set; }
}