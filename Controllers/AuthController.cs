using Microsoft.AspNetCore.Mvc;
using URLshortner.Dtos;
using URLshortner.Services;
using URLshortner.Exceptions;

namespace URLshortner.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequestDto dto)
    {
        try
        {
            var token = await _authService.Login(dto);
            var response = new ApiResponse(token, "Login successful", 200);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse(ex.Message, 500);
            return StatusCode(500, response);
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequestDto dto)
    {
        try
        {
            await _authService.Register(dto);
            var response = new ApiResponse("Registration successful", 200);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse(ex.Message, 500);
            return StatusCode(500, response);
        }
    }
}