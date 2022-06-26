namespace LabeledGallery.Dto.Gallery;

public class AddGalleryItemDetectedObjectsRequestDto
{
    public string GalleryItemId { get; set; }
    public List<string> DetectedObjects { get; set; } = new();
}