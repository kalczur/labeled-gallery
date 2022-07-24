using System.ComponentModel.DataAnnotations;

namespace LabeledGallery.Dto.User;

public class LoginRequestDto
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
