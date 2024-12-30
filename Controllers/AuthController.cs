using Microsoft.AspNetCore.Mvc;
using URLshortner.Dtos;
using URLshortner.Services;
using URLshortner.Exceptions; // Importing custom exceptions

namespace URLshortner.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService ?? throw new ArgumentNullException(nameof(authService));

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequestDto dto)
    {
        try
        {
            var token = await _authService.Login(dto);
            return Ok(new { token, message = "Login successful" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequestDto dto)
    {
        try
        {
            var message = await _authService.Register(dto);
            return Ok(new { message = message, status = "Registration successful" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
        }
    }
}