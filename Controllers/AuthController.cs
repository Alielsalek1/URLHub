using Microsoft.AspNetCore.Mvc;
using URLshortner.Exceptions;
using System.Security.Claims;
using URLshortner.Services.Implementations;
using URLshortner.Services.Interfaces;
using URLshortner.Dtos.Implementations;

namespace URLshortner.Controllers;

// TODO: OAuth2

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService, ITokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var token = await authService.LoginAsync(dto);
        var response = new ApiResponse("Login successful", 200, token);
        return StatusCode(200, response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        await authService.RegisterAsync(dto);
        var response = new ApiResponse("Registration successful", 201);
        return StatusCode(201, response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequest dto)
    {
        var token = await tokenService.RefreshAsync(dto);
        var response = new ApiResponse("Token refreshed successfully", 200, token);
        return StatusCode(200, response);
    }
}