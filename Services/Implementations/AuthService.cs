using URLshortner.Dtos;
using URLshortner.Exceptions;
using URLshortner.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using URLshortner.Repositories.Interfaces;
using URLshortner.Utils;
using URLshortner.Services.Interfaces;
using Microsoft.JSInterop.Infrastructure;
using Microsoft.Extensions.Configuration;
using URLshortner.Dtos.Implementations;
using URLshortner.Repositories.Implementations;

namespace URLshortner.Services.Implementations;

public class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IActivationTokenRepository activationTokenRepository,
    IMapper mapper,
    ITokenService tokenService,
    IUserService userService,
    IEmailService emailService,
    IConfiguration configuration
    ) : IAuthService
{
    public async Task<AuthResponse> LoginAsync(LoginRequest dto)
    {
        var curUser = await userRepository.GetByUsernameAsync(dto.username);
        
        if (!Helpers.IsValidUser(curUser, dto.password))
        {
            throw new InvalidInputException("Invalid Credentials");
        }

        var refreshToken = await refreshTokenRepository.GetRefreshTokenByUserIdAsync(curUser.Id);
        if (refreshToken == null)
        {
            throw new NotFoundException("no refresh token found");
        }
        if (refreshToken.IsRevoked)
        {
            throw new UnAuthorizedException("refresh token is revoked");
        }
        if (refreshToken.Expires < DateTime.UtcNow)
        {
            throw new UnAuthorizedException("refresh token has been expired");
        }

        await tokenService.RenewRefreshTokenAsync(refreshToken);

        var accessToken = tokenService.GenerateAccessToken(curUser);

        return new AuthResponse
        {
            UserId = curUser.Id,
            RefreshToken = refreshToken.Token,
            AccessToken = accessToken
        };
    }

    public async Task<UserResponse> RegisterAsync(RegisterRequest dto)
    {
        // getting the mapped user from the DTO
        User newUser = mapper.Map<User>(dto);

        // checking if the username already exists, username should be unique
        if (await userRepository.GetByUsernameAsync(dto.username) != null)
        {
            throw new AlreadyExistsException("username already exists");
        }

        var user = await userRepository.GetByEmailAsync(dto.email);
        // checking if the Email already exists
        if (user != null)
        {
            if (user.IsEmailVerified)
            {
                throw new AlreadyExistsException("Email already in use");
            }
            else
            {
                throw new ExistsButNotVerifiedException("Email already exists but requires verification");
            }
        }

        await userService.AddUserAsync(newUser);

        // create refresh token
        await tokenService.GenerateRefreshTokenAsync(newUser);

        var userFromDb = await userRepository.GetByUsernameAsync(newUser.Username);
        var response = mapper.Map<UserResponse>(userFromDb);

        return response;
    }

    public async Task RequestActivationAsync(ActivationRequest dto)
    {
        var user = await userRepository.GetByIdAsync(dto.userId);
        if (user == null)
            throw new NotFoundException();
        if (user.IsEmailVerified)
            throw new AlreadyVerifiedException();

        var activationToken = new ActivationToken
        {
            Token = new Guid(),
            User = user,
            UserId = dto.userId,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        await activationTokenRepository.AddAsync(activationToken);

        string activationLink = 
            $"{configuration["server"]}/auth/activate" +
            $"?token={activationToken.Token}&email={user.Email}";

        string subject = "Activate Your Account";
        string body = $"<h1>Welcome to URL Hub!</h1>" +
            $"<p>Please activate your account by clicking <a href='{activationLink}'>here</a>.</p>";

        await emailService.SendEmailAsync(user.Email, subject, body);
    }

    public async Task ActivateUserAsync(ApplyActivationRequest dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.email);
        if (user == null)
        {
            throw new NotFoundException("no user found with this Email");
        }
        if (user.IsEmailVerified)
        {
            throw new AlreadyVerifiedException("user is already verified");
        }

        var activationToken = await activationTokenRepository.GetByUserIdAsync(user.Id, dto.token);
        if (activationToken == null)
        {
            throw new InvalidInputException("Not a valid activationToken");
        }

        user.IsEmailVerified = true;
        await userRepository.UpdateAsync(user);
    }
}