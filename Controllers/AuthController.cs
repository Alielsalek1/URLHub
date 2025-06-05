using Microsoft.AspNetCore.Mvc;
using URLshortner.Exceptions;
using System.Security.Claims;
using URLshortner.Services.Implementations;
using URLshortner.Services.Interfaces;
using URLshortner.Dtos.Implementations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using URLshortner.Enums;
using Microsoft.AspNetCore.Authorization;

namespace URLshortner.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService, ITokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var token = await authService.LoginAsync(dto);
        var response = new ApiResponse("Login successful", 200, token);
        return StatusCode(200, response);
    }

    [HttpGet("externallogin")]
    public async Task<IActionResult> ExternalLogin()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(ExternalLoginCallback)) };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("externallogincallback")]
    public async Task<IActionResult> ExternalLoginCallback()
    {
        var content = await authService.OauthLogin(HttpContext);
        var response = new ApiResponse("Login successful", 200, content);
        return StatusCode(200, response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        var content = await authService.RegisterAsync(dto);
        var response = new ApiResponse("Registration successful, Complete your verification", 201, content);
        return StatusCode(201, response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequest dto)
    {
        var token = await tokenService.RefreshAsync(dto);
        var response = new ApiResponse("Token refreshed successfully", 200, token);
        return StatusCode(200, response);
    }

    [HttpPost("request-reset-password")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] ActionRequest dto)
    {
        await authService.RequestPasswordReset(dto);
        var response = new ApiResponse("Password reset email sent successfully", 200);
        return StatusCode(200, response);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest dto)
    {
        var user = await authService.ResetPassword(dto);
        var response = new ApiResponse("Password reset successfully", 200, user);
        return StatusCode(200, response);
    }

    [HttpPost("request-activate-email")]
    public async Task<IActionResult> RequestActivation([FromBody] ActionRequest dto)
    {
        await authService.RequestActivationAsync(dto);
        var response = new ApiResponse("Activation Email sent successfully", 200);
        return StatusCode(200, response);
    }

    [HttpPost("activate-email")]
    public async Task<IActionResult> Activate([FromQuery] ApplyActivationRequest dto)
    {
        await authService.ActivateUserAsync(dto);
        var response = new ApiResponse("Account activated successfully.", 200);
        return StatusCode(200, response);
    }
}