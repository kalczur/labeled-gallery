namespace LabeledGallery.Dto.Gallery;

public class ModifyGalleryItemDetectedObjectRequestDto
{
    public string GalleryItemId { get; set; }
    public string DetectedObjectNameBefore { get; set; }
    public string DetectedObjectNameAfter { get; set; }
}