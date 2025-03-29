using URLshortner.Dtos;
using URLshortner.Dtos.Implementations;

namespace URLshortner.Services.Interfaces;

public interface IAuthService
{
    Task<UserResponse> RegisterAsync(RegisterRequest dto);
    Task<AuthResponse> LoginAsync(LoginRequest dto);
    Task RequestActivationAsync(ActivationRequest dto);
    Task ActivateUserAsync(ApplyActivationRequest dto);
}
