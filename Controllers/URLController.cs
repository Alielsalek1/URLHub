using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLshortner.Dtos;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Services;

namespace URLshortner.Controllers;

[Authorize]
[ApiController]
[Route("URL")]
public class UrlController(UrlService urlService) : ControllerBase
{
    private readonly UrlService _urlService = urlService ?? throw new ArgumentNullException(nameof(urlService));
    
    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetMyUrls(int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    // {
    //     var urls = await _urlService.GetMyUrls(id, pageNumber, pageSize);
    //
    //     if (urls == null || !urls.Any())
    //     {
    //         return Ok(new ApiResponse("No URLs found for this user", 200));
    //     }
    //
    //     return Ok(new ApiResponse(urls, "URLs retrieved successfully", 200));
    // }
    
    [HttpPost]
    public async Task<IActionResult> AddUrl([FromBody] UrlRequestDTO requestDto)
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return BadRequest(new ApiResponse("Invalid User ID format", 400));
            }
            
            await _urlService.AddUrl(requestDto, userId);
            return Ok(new ApiResponse("URL added successfully", 200));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(ex.Message, 400));
        }
        
    }

    // DELETE /api/urls
    [HttpDelete]
    public async Task<IActionResult> RemoveUrl([FromBody] UrlRequestDTO requestDto)
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return BadRequest(new ApiResponse("Invalid User ID format", 400));
            }
            
            await _urlService.RemoveUrl(requestDto, userId);
            return Ok(new ApiResponse("URL removed successfully", 200));
        }
        catch
        {
            return NotFound(new ApiResponse($"URL not found", 404));
        }
        
    }
}
