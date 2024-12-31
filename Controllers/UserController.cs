using System.Security.Claims;
using ALL.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using URLshortner.Dtos;
using URLshortner.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Services;

namespace URLshortner.Controllers;

[Authorize]
[ApiController]
[Route("user")]
public class UserController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            return Ok(new ApiResponse(user, "User fetched successfully", 200));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse(ex.Message, 500));
        }
    }
    
    [HttpPut("me")]
    public async Task<ActionResult<ApiResponse>> UpdateUser([FromBody] UpdateUserRequestDto dto)
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return BadRequest(new ApiResponse("Invalid User ID format", 400));
            }
            
            await _userService.UpdateUser(userId, dto);
            return Ok(new ApiResponse("User updated successfully", 200));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse(ex.Message, 500));
        }
    }
}
