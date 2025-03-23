using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLshortner.Dtos;
using URLshortner.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Services.Implementations;
using URLshortner.Services.Interfaces;

namespace URLshortner.Controllers;

[Authorize]
[ApiController]
[Route("friend")]
public class UserFriendController(IUserFriendService friendService) : ControllerBase
{
    [HttpPost("{id}")]
    public async Task<IActionResult> AddFriend(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(userIdClaim, out int myId);

        await friendService.AddItemAsync(myId, id);
        var response = new ApiResponse("Friend added successfully", 204);

        return StatusCode(204, response);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetItems([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(userIdClaim, out int userId);

        var pagedItems = await friendService.GetItemsPagedAsync(userId, pageNumber, pageSize);
        var response = new ApiResponse($"got page no.{pageNumber} successfully", 200, pagedItems);
        return StatusCode(200, response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> DeleteFriend(int id)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return BadRequest(new ApiResponse("Invalid User ID format", 400));
        }

        await friendService.DeleteItemAsync(userId, id);
        return Ok(new ApiResponse("Friend deleted successfully", 204));
    }
}