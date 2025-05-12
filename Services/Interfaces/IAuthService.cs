using URLshortner.Dtos;
using URLshortner.Dtos.Implementations;
using URLshortner.Enums;

namespace URLshortner.Services.Interfaces;

public interface IAuthService
{
    Task<UserResponse> RegisterAsync(RegisterRequest dto);
    Task<AuthResponse> LoginAsync(LoginRequest dto, AuthScheme authScheme = AuthScheme.UrlHub);
    Task RequestActivationAsync(ActivationRequest dto);
    Task ActivateUserAsync(ApplyActivationRequest dto);
    Task<AuthResponse> OauthLogin(HttpContext context);
}
