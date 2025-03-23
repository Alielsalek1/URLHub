using URLshortner.Dtos;
using URLshortner.Dtos.Implementations;

namespace URLshortner.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest dto);
    Task<AuthResponse> LoginAsync(LoginRequest dto);
}
