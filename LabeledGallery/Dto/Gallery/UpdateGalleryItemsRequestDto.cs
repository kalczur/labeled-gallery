namespace LabeledGallery.Dto.Gallery;

public class UpdateGalleryItemsRequestDto
{
    public string Name { get; set; } 
    public List<IFormFile> ImagesToAdd { get; set; } 
    public List<string> GalleryItemIdsToRemove { get; set; } 
}