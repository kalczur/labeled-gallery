using System.ComponentModel.DataAnnotations;
using LabeledGallery.Models.Gallery;

namespace LabeledGallery.Dto.User;

public class RegisterRequestDto
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public ObjectsDetectionProvider? ObjectsDetectionProvider { get; set; }
}