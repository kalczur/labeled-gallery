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

    [Route("modify-detected-objects")]
    [HttpPost]
    public async Task<IActionResult> ModifyDetectedObjects(ModifyDetectedObjectsRequestDto dto)
    {
        if (dto.DetectedObjects.Count is 0 or > 20)
            return BadRequest("It is only possible to add between 1 and 20 labels.");

        var succeed = await _galleryService.ModifyDetectedObjects(dto, AccountEmail);
        if (succeed == false) return BadRequest();

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
