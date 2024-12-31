using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLshortner.Dtos;
using URLshortner.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Services;

namespace URLshortner.Controllers;

[Authorize]
[ApiController]
[Route("friend")]
public class FriendController(FriendService friendService) : ControllerBase
{
    private readonly FriendService _friendService = friendService  ?? throw new ArgumentNullException(nameof(friendService)); 

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> AddFriend([FromBody] FriendRequestDTO? dto)
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return BadRequest(new ApiResponse("Invalid User ID format", 400));
            }
            
            await _friendService.AddFriend(userId, dto);
            return Ok(new ApiResponse("Friend updated successfully", 200));
        }
        catch (Exception ex)
        {
            return NotFound(new ApiResponse(ex.Message, 500));
        }
    }
    
    // [HttpGet]
    // [Route("{id}")]
    // public async Task<ActionResult<List<int>>> GetMyFriends(int? id)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest("Invalid Credentials");
    //     }
    //
    //     var IsUserFound = await _userRepository.GetUserById(id);
    //
    //     if (IsUserFound == null)
    //     {
    //         return NotFound($"User with ID {id} isn't registered");
    //     }
    //
    //     var MyFriends = await _repository.GetFriendsById(id);
    //
    //     return Ok(new { Message = $"Friends for user {id} retrieved successfully!", friends = MyFriends });
    // }
    
    [HttpDelete]
    [Route("")]
    public async Task<ActionResult<string>> DeleteFriend([FromBody] FriendRequestDTO? dto)
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return BadRequest(new ApiResponse("Invalid User ID format", 400));
            }

            await _friendService.DeleteFriend(userId, dto);
            return Ok(new ApiResponse("Friend deleted successfully", 200));
        }
        catch (InvalidUserException ex)
        {
            return NotFound(new ApiResponse(ex.Message, 404));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(ex.Message, 400));
        }
    }
}