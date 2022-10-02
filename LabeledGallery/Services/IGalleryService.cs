using LabeledGallery.Dto.Gallery;

namespace LabeledGallery.Services;

public interface IGalleryService
{
    Task UpdateGalleryItems(UpdateGalleryItemsRequestDto dto, string accountEmail);
}
