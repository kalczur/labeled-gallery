using Google.Cloud.Vision.V1;
using LabeledGallery.Dto.Gallery;
using LabeledGallery.Models.Gallery;
using LabeledGallery.Models.User;
using LabeledGallery.Utils;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Attachments;

namespace LabeledGallery.Services;

public class GalleryService : IGalleryService
{
    private readonly ImageAnnotatorClient _imageAnnotatorClient;
    private readonly DocumentStoreHolder _storeHolder;

    public GalleryService(ImageAnnotatorClient imageAnnotatorClient, DocumentStoreHolder storeHolder)
    {
        _imageAnnotatorClient = imageAnnotatorClient;
        _storeHolder = storeHolder;
    }

    public async Task UpdateGalleryItems(UpdateGalleryItemsRequestDto dto, string accountEmail)
    {
        string galleryId;
        using (var session = _storeHolder.OpenAsyncSession())
        {
            var account = await session.Query<Account>().SingleAsync(x => x.Email == accountEmail);
            galleryId = account.GalleryId;
        }

        foreach (var imageFile in dto.ImagesToAdd)
        {
            var imageStream = await GetImageStream(imageFile);
            var image = await Image.FromStreamAsync(imageStream);
            imageStream.Position = 0;

            var result = await _imageAnnotatorClient.DetectLabelsAsync(image);

            var detectedObject = result
                .Select(x => new DetectedObject
                {
                    Label = x.Description,
                    Accuracy = x.Score
                })
                .ToList();

            var fileName = imageFile.FileName;
            var splitFileName = fileName.Split(".");

            var galleryItem = new GalleryItem
            {
                GalleryId = galleryId,
                Name = splitFileName[0],
                DetectedObjects = detectedObject
            };

            using (var session = _storeHolder.OpenAsyncSession())
            {
                await session.StoreAsync(galleryItem);
                session.Advanced.Attachments.Store(galleryItem.Id, fileName, imageStream, splitFileName[1]);

                var gallery = await session.LoadAsync<Gallery>(galleryId);
                gallery.GalleryItemIds.Add(galleryItem.Id);

                await session.SaveChangesAsync();
            }
        }
    }

    public async Task<GalleryResponseDto> GetGallery(GetGalleryRequestDto dto, string accountEmail)
    {
        List<GalleryItem> galleryItems;
        var searchKeyword = string.IsNullOrEmpty(dto.SearchKeyword) ? "" : dto.SearchKeyword;

        using (var session = _storeHolder.OpenAsyncSession())
        {
            var account = await session.Query<Account>().SingleAsync(x => x.Email == accountEmail);
            var gallery = await session.LoadAsync<Gallery>(account.GalleryId);

            galleryItems = await session.Query<GalleryItem>()
                .Where(x =>
                    x.GalleryId == gallery.Id &&
                    x.DetectedObjects.Any(y => y.Label.Contains(searchKeyword)))
                .ToListAsync();
        }

        var galleryDto = new GalleryResponseDto();

        foreach (var galleryItem in galleryItems)
        {
            using (var session = _storeHolder.OpenAsyncSession())
            {
                var attachmentRequest = new List<AttachmentRequest> { new(galleryItem.Id, galleryItem.Name + ".jpg") };

                var attachments = await session.Advanced.Attachments.GetAsync(attachmentRequest);
                attachments.MoveNext();

                galleryDto.GalleryItems.Add(new GalleryItemResponseDto
                {
                    Name = galleryItem.Name,
                    DetectedObjects = galleryItem.DetectedObjects,
                    Image = GetImageUrl(galleryItem.Id, galleryItem.Name)
                });
            }
        }

        return galleryDto;
    }

    private static async Task<MemoryStream> GetImageStream(IFormFile imageFile)
    {
        MemoryStream imageStream;
        using (var memoryStream = new MemoryStream())
        {
            await imageFile.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            imageStream = new MemoryStream(imageBytes);
        }

        return imageStream;
    }

    // TODO just temp solution - it can't be done that way!
    private static string GetImageUrl(string id, string name)
    {
        return
            $"http://10.0.2.2:8080/databases/labeled-gallery/attachments?id=GalleryItems%2F{id.Split("/")[1]}&name={name}.jpg";
    }
}
