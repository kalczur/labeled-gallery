﻿using Google.Cloud.Vision.V1;
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
        List<string> existingImages;
        using (var session = OpenAsyncSession())
        {
            var account = await session.Query<Account>().SingleAsync(x => x.Email == accountEmail);
            galleryId = account.GalleryId;
            existingImages = await session.Query<GalleryItem>().Where(x => x.GalleryId == galleryId)
                .Select(x => x.Name + ".jpg")
                .ToListAsync();
        }

        var imagesToAdd = dto.ImagesToAdd.Where(x => !existingImages.Contains(x.FileName));

        foreach (var imageFile in imagesToAdd)
        {
            var imageStream = await GetImageStream(imageFile);
            var image = await Image.FromStreamAsync(imageStream);
            imageStream.Position = 0;

            var result = await _imageAnnotatorClient.DetectLabelsAsync(image);

            var detectedObjects = result
                .Select(x => new DetectedObject
                {
                    Id = Guid.NewGuid().ToString(),
                    Label = x.Description,
                    Accuracy = x.Score,
                    DetectionProvider = DetectionProvider.Gcp
                })
                .ToList();

            var fileName = imageFile.FileName;
            var splitFileName = fileName.Split(".");

            var galleryItem = new GalleryItem
            {
                GalleryId = galleryId,
                Name = splitFileName[0],
                DetectedObjects = detectedObjects
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
                .Where(x => x.GalleryId == gallery.Id)
                .ToListAsync();

            sortedGalleryItems = galleryItems
                .Where(x => x.DetectedObjects.Any(y =>
                    y.Label.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(x =>
                    x.DetectedObjects.Where(y => y.Label.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase))
                        .Sum(y => y.Accuracy))
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
                    .Where(x => x.Label.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase))
                    .Sum(x => x.Accuracy);

                galleryDto.GalleryItems.Add(new GalleryItemResponseDto
                {
                    Id = galleryItem.Id,
                    Name = galleryItem.Name,
                    DetectedObjects = galleryItem.DetectedObjects,
                    Image = GetImageUrl(galleryItem.Id, galleryItem.Name),
                    TotalAccuracy = totalAccuracy
                });
            }
        }

        return galleryDto;
    }

    public async Task<bool> ModifyDetectedObjects(ModifyDetectedObjectsRequestDto dto, string accountEmail)
    {
        using (var session = OpenAsyncSession())
        {
            var galleryItem = await GetGalleryItem(dto.GalleryItemId, accountEmail, session);
            if (galleryItem == null)
                return false;

            galleryItem.DetectedObjects = dto.DetectedObjects;
            await session.SaveChangesAsync();
        }

        return true;
    }

    private static async Task<GalleryItem?> GetGalleryItem(string id, string accountEmail,
        IAsyncDocumentSession session)
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
