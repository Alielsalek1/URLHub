using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ALL.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using URLshortner.Dtos;
using URLshortner.Domain.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Services;
using URLshortner.Services.Interfaces;


namespace URLshortner.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetUserById()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(userIdClaim, out int userId);

        var user = await userService.GetUserByIdAsync(userId);
        var response = new ApiResponse("Profile retrieved successfully", 200, user);
        return StatusCode(200, response);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(userIdClaim, out int userId);

        var user = await userService.UpdateUserAsync(userId, dto);
        var response = new ApiResponse("user updated successfully", 200, user);
        return StatusCode(200, response);
    }
}
