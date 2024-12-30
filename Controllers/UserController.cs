using ALL.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using URLshortner.Dtos;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Services;

namespace URLshortner.Controllers;

[ApiController]
[Route("/user")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpDelete]
    [Route("/me")]
    [Authorize]
    public async Task<ActionResult<string>> DeleteUser()
    {
        var userIdString = User.FindFirst("sub")?.Value;
        int userId;
        if (int.TryParse(userIdString, out userId))
        {
            try
            {
                await userService.RemoveUser(userId);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }
        else
        {
            return BadRequest("Invalid UserId");
        }
    }

    [HttpGet]
    [Route("/{id}")]
    [Authorize]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        try
        {
            var user = userService.GetUserById(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    [Route("/me")]
    [Authorize]
    public async Task<ActionResult<string>> UpdateUser([FromBody] UpdateUserRequestDto dto)
    {
        var userIdString = User.FindFirst("sub")?.Value;
        int userId;
        if (int.TryParse(userIdString, out userId))
        {
            try
            {
                await userService.UpdateUser(userId, dto);
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }
        else
        {
            return BadRequest("Invalid UserId");
        }
    }
}
