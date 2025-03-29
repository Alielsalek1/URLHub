using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLshortner.Dtos.Implementations;
using URLshortner.Services.Interfaces;

namespace URLshortner.Controllers;

[Authorize]
[ApiController]
[Route("url")]
public class UrlController(IUrlService urlService) : ControllerBase
{
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUrls(int id, [FromQuery] PaginationRequest dto)
    {
        var Urls = await urlService.GetItemsPagedAsync(id, dto.pageNumber, dto.pageSize);
        var response = new ApiResponse($"got page no.{dto.pageNumber} successfully", 200, Urls);

        return StatusCode(200, response);
    }

    [HttpPost("me")]
    public async Task<IActionResult> AddUrl([FromBody] UrlRequest dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(userIdClaim, out int userId);

        await urlService.AddItemAsync(dto, userId);

        var response = new ApiResponse("URL added successfully", 201);
        return StatusCode(201, response);
    }

    [HttpDelete("me")]
    public async Task<IActionResult> RemoveUrl([FromBody] UrlRequest dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(userIdClaim, out int userId);

        await urlService.DeleteItemAsync(dto.url, userId);

        var response = new ApiResponse("URL removed successfully", 204);
        return StatusCode(204, response);
    }

    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> RedirectToOriginal(string shortUrl)
    {
        var originalUrl = await urlService.GetOriginalUrlAsync(shortUrl);
        return Redirect(originalUrl);
    }
}
