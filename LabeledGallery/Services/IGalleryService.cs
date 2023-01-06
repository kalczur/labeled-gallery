using LabeledGallery.Dto.Gallery;

namespace LabeledGallery.Services;

public interface IGalleryService
{
    Task UpdateGalleryItems(UpdateGalleryItemsRequestDto dto, string accountEmail);
    Task<GalleryResponseDto> GetGallery(GetGalleryRequestDto dto, string accountEmail);
    Task<bool> ModifyDetectedObjects(ModifyDetectedObjectsRequestDto dto, string accountEmail);
}