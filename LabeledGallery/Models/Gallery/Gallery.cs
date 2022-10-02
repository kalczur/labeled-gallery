namespace LabeledGallery.Models.Gallery;

public class Gallery
{
    public string Id { get; set; }
    public string AccountId { get; set; }
    public List<string> GalleryItemIds { get; set; } = new();
}
