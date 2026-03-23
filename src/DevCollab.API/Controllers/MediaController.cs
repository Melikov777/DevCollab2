using DevCollab.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevCollab.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MediaController : ControllerBase
{
    private readonly IMediaStorageService _mediaStorageService;

    public MediaController(IMediaStorageService mediaStorageService)
    {
        _mediaStorageService = mediaStorageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        using var stream = file.OpenReadStream();

        var uploadedFileName = await _mediaStorageService.UploadAsync(stream, fileName, file.ContentType);
        return Ok(new { fileName = uploadedFileName });
    }

    [HttpGet("url/{fileName}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUrl(string fileName)
    {
        var url = await _mediaStorageService.GetUrlAsync(fileName);
        return Ok(new { url });
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> Delete(string fileName)
    {
        await _mediaStorageService.DeleteAsync(fileName);
        return NoContent();
    }
}
