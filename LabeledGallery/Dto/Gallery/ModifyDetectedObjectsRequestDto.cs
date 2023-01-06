using LabeledGallery.Models.Gallery;

namespace LabeledGallery.Dto.Gallery;

public class ModifyDetectedObjectsRequestDto
{
    public string GalleryItemId { get; set; }
    public List<DetectedObject> DetectedObjects { get; set; } = new();
}