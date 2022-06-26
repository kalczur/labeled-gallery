namespace LabeledGallery.Models.User;

public class Account
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public AccountOptions Options { get; set; }
    public string GalleryId { get; set; }
}