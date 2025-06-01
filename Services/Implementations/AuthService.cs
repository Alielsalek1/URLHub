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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using URLshortner.Enums;

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
    public async Task<AuthResponse> LoginAsync(LoginRequest dto, AuthScheme authScheme)
    {
        var curUser = await userRepository.GetByUsernameAsync(dto.username);
        
        if (!Helpers.IsValidUser(curUser, dto.password) || curUser.AuthScheme != authScheme)
        {
            throw new InvalidInputException("Invalid Credentials");
        }

        var refreshToken = await refreshTokenRepository.GetRefreshTokenByUserIdAsync(curUser.Id);
        if (refreshToken == null)
        {
            throw new NotFoundException("Refresh token not found.");
        }
        await tokenService.RenewRefreshTokenAsync(refreshToken);
        Helpers.IsValidRefreshToken(refreshToken);

        var accessToken = tokenService.GenerateJwtToken(curUser);

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
        newUser.AuthScheme = AuthScheme.UrlHub;
        
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

    public async Task<AuthResponse> OauthLogin(HttpContext context)
    {
        // Retrieve the external login information after the user has authenticated with Google.
        var result = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            throw new GoogleAuthFailedException("External authentication failed.");
        }

        // user Data
        var externalPrincipal = result.Principal;
        string email = externalPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? "";
        string name = externalPrincipal.FindFirst(ClaimTypes.Name)?.Value ?? "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
        {
            throw new InvalidInputException("Invalid external login information.");
        }

        var user = await userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            // registeration
            var passwordHasher = new PasswordHasher<string>(); // for storing hashed passwords in the db
            user = new User
            {
                Username = name,
                Email = email,
                IsEmailVerified = true,
                AuthScheme = AuthScheme.Google,
                Password = passwordHasher.HashPassword(null, "0")
            };
            await userRepository.AddAsync(user);
            await tokenService.GenerateRefreshTokenAsync(user);
        }

        return await this.LoginAsync(new LoginRequest
        {
            username = user.Username,
            password = "0"
        },
        AuthScheme.Google);
    }
}