using LabeledGallery.Dto.Gallery;
using LabeledGallery.Models.Gallery;
using Microsoft.AspNetCore.Mvc;

namespace LabeledGallery.Controllers;

[ApiController]
[Route("/api/v1/")]
public class GalleryController : ControllerBase
{
    [Route("gallery")]
    [HttpGet]
    public async Task<IActionResult> GetGallery()
    {
        // TODO - implement
        return Ok(GetValidGalleryResponse());
    }
    
    [Route("add-gallery-item-detected-objects")]
    [HttpPost]
    public async Task<IActionResult> AddGalleryItemDetectedObject(AddGalleryItemDetectedObjectsRequestDto dto)
    {
        // TODO - implement
        return Ok();
    }
    
    [Route("modify-gallery-item-detected-object")]
    [HttpPost]
    public async Task<IActionResult> ModifyGalleryItemDetectedObject(ModifyGalleryItemDetectedObjectRequestDto dto)
    {
        // TODO - implement
        return Ok();
    }

    [Route("update-gallery-items")]
    [HttpPost]
    public async Task<IActionResult> UpdateGalleryItems(UpdateGalleryItemsRequestDto dto)
    {
        // TODO - implement
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