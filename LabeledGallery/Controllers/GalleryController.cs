using LabeledGallery.Dto.Gallery;
using LabeledGallery.Models.Gallery;
using LabeledGallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabeledGallery.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/gallery/")]
public class GalleryController : AbstractController
{
    private readonly IGalleryService _galleryService;

    public GalleryController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    [Route("get")]
    [HttpGet]
    public async Task<IActionResult> GetGallery()
    {
        // TODO - implement
        return Ok(GetValidGalleryResponse());
    }

    [Route("add-detected-objects")]
    [HttpPost]
    public async Task<IActionResult> AddDetectedObject(AddGalleryItemDetectedObjectsRequestDto dto)
    {
        // TODO - implement
        return Ok();
    }

    [Route("modify-detected-object")]
    [HttpPost]
    public async Task<IActionResult> ModifyDetectedObject(ModifyGalleryItemDetectedObjectRequestDto dto)
    {
        // TODO - implement
        return Ok();
    }

    [Route("update")]
    [HttpPost]
    public async Task<IActionResult> Update([FromForm] UpdateGalleryItemsRequestDto dto)
    {
        await _galleryService.UpdateGalleryItems(dto, AccountEmail);
        return Ok();
    }

    // STUBS

    private static GalleryResponseDto GetValidGalleryResponse()
    {
        return new GalleryResponseDto
        {
            GalleryItems = new List<GalleryItemResponseDto>
            {
                new()
                {
                    Id = "id-1",
                    Name = "g-item-1",
                    Url = "https://some-url-1.net",
                    TotalAccuracy = 1.86f,
                    DetectedObjects = new List<DetectedObject>
                    {
                        new()
                        {
                            Label = "label-1A",
                            Accuracy = 0.98f
                        },
                        new()
                        {
                            Label = "label-2A",
                            Accuracy = 0.88f
                        }
                    }
                },
                new()
                {
                    Id = "id-2",
                    Name = "g-item-2",
                    Url = "https://some-url-2.net",
                    TotalAccuracy = 2.1f,
                    DetectedObjects = new List<DetectedObject>
                    {
                        new()
                        {
                            Label = "label-1B",
                            Accuracy = 0.6f
                        },
                        new()
                        {
                            Label = "label-2B",
                            Accuracy = 0.7f
                        },
                        new()
                        {
                            Label = "label-3B",
                            Accuracy = 0.8f
                        }
                    }
                }
            }
        };
    }
}
