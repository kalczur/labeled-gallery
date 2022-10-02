using LabeledGallery.Models.Gallery;

namespace LabeledGallery.Dto.Gallery;

public class GalleryItemResponseDto
{
    public string Name { get; set; }
    public string Image { get; set; }
    public List<DetectedObject> DetectedObjects { get; set; } = new();
}
