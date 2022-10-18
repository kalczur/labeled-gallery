using LabeledGallery.Dto.Gallery;
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
    [HttpPost]
    public async Task<IActionResult> GetGallery(GetGalleryRequestDto dto)
    {
        var galleryDto = await _galleryService.GetGallery(dto, AccountEmail);
        return Ok(galleryDto);
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
}
