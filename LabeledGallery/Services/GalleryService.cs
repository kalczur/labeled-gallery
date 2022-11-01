using Google.Cloud.Vision.V1;
using LabeledGallery.Dto.Gallery;
using LabeledGallery.Models.Gallery;
using LabeledGallery.Models.User;
using LabeledGallery.Utils;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Attachments;
using Raven.Client.Documents.Session;

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

    private IAsyncDocumentSession OpenAsyncSession() => _storeHolder.OpenAsyncSession();

    public async Task UpdateGalleryItems(UpdateGalleryItemsRequestDto dto, string accountEmail)
    {
        string galleryId;
        using (var session = OpenAsyncSession())
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

            using (var session = OpenAsyncSession())
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
        List<GalleryItem> sortedGalleryItems;
        var searchKeyword = string.IsNullOrEmpty(dto.SearchKeyword) ? "" : dto.SearchKeyword;

        using (var session = OpenAsyncSession())
        {
            var account = await session.Query<Account>().SingleAsync(x => x.Email == accountEmail);
            var gallery = await session.LoadAsync<Gallery>(account.GalleryId);

            var galleryItems = await session.Query<GalleryItem>()
                .Where(x =>
                    x.GalleryId == gallery.Id &&
                    x.DetectedObjects.Any(y => y.Label.Contains(searchKeyword)))
                .ToListAsync();

            sortedGalleryItems = galleryItems.OrderByDescending(x =>
                    x.DetectedObjects.Where(y => y.Label.Contains(searchKeyword)).Sum(y => y.Accuracy))
                .ToList();
        }

        var galleryDto = new GalleryResponseDto();

        foreach (var galleryItem in sortedGalleryItems)
        {
            using (var session = OpenAsyncSession())
            {
                var attachmentRequest = new List<AttachmentRequest> { new(galleryItem.Id, galleryItem.Name + ".jpg") };

                var attachments = await session.Advanced.Attachments.GetAsync(attachmentRequest);
                attachments.MoveNext();

                var totalAccuracy = galleryItem.DetectedObjects
                    .Where(x => x.Label.Contains(dto.SearchKeyword))
                    .Sum(x => x.Accuracy);

                galleryDto.GalleryItems.Add(new GalleryItemResponseDto
                {
                    Name = galleryItem.Name,
                    DetectedObjects = galleryItem.DetectedObjects,
                    Image = GetImageUrl(galleryItem.Id, galleryItem.Name),
                    TotalAccuracy = totalAccuracy
                });
            }
        }

        return galleryDto;
    }

    public async Task<bool> AddDetectedObject(AddGalleryItemDetectedObjectsRequestDto dto, string accountEmail)
    {
        using (var session = OpenAsyncSession())
        {
            var galleryItem = await GetGalleryItem(dto.GalleryItemId, accountEmail, session);
            if (galleryItem == null)
                return false;

            if (galleryItem.DetectedObjects.Exists(x => x.Label == dto.DetectedObject))
                return false;

            galleryItem.DetectedObjects.Add(new DetectedObject
            {
                Accuracy = 1,
                Label = dto.DetectedObject
            });

            await session.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> ModifyDetectedObject(ModifyGalleryItemDetectedObjectRequestDto dto, string accountEmail)
    {
        using (var session = OpenAsyncSession())
        {
            var galleryItem = await GetGalleryItem(dto.GalleryItemId, accountEmail, session);
            if (galleryItem == null)
                return false;

            var detectedObject = galleryItem.DetectedObjects.SingleOrDefault(x => x.Label == dto.DetectedObjectNameBefore);
            if (detectedObject == null)
                return false;

            detectedObject.Label = dto.DetectedObjectNameAfter;
            detectedObject.Accuracy = 1;

            await session.SaveChangesAsync();
        }

        return true;
    }

    private static async Task<GalleryItem?> GetGalleryItem(string id, string accountEmail, IAsyncDocumentSession session)
    {
        var account = await session.Query<Account>().SingleAsync(x => x.Email == accountEmail);
        var gallery = await session.LoadAsync<Gallery>(account.GalleryId);

        var galleryItemId = gallery.GalleryItemIds.SingleOrDefault(x => x == id);

        if (galleryItemId == null) return null;

        return await session.LoadAsync<GalleryItem>(galleryItemId);
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

    private static string GetImageUrl(string id, string name)
    {
        return
            $"http://10.0.2.2:8080/databases/labeled-gallery/attachments?id=GalleryItems%2F{id.Split("/")[1]}&name={name}.jpg";
    }
}
