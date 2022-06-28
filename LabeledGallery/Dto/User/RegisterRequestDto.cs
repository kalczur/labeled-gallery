using System.ComponentModel.DataAnnotations;
using LabeledGallery.Models.Gallery;

namespace LabeledGallery.Dto.User;

public class RegisterRequestDto
{
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    [MinLength(4)]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string Password { get; set; }
    
    [Required]
    public ObjectsDetectionProvider? ObjectsDetectionProvider { get; set; }
}