namespace LabeledGallery.Dto.Gallery;

public class ModifyGalleryItemDetectedObjectRequestDto
{
    public string GalleryItemId { get; set; }
    public string DetectedObjectNameBefore;
    public string DetectedObjectNameAfter;
}