using LabeledGallery.Models.Gallery;

namespace LabeledGallery.Dto.Gallery;

public class GalleryItemResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public float TotalAccuracy { get; set; }
    public List<DetectedObject> DetectedObjects { get; set; } = new List<DetectedObject>();
}