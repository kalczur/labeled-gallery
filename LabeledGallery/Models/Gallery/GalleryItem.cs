namespace LabeledGallery.Models.Gallery;

public class GalleryItem
{
    public string Id { get; set; }
    public string GalleryId { get; set; }
    public string Name { get; set; }
    public List<DetectedObject> DetectedObjects { get; set; } = new();
}
