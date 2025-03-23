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

namespace URLshortner.Services.Implementations;

public class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IMapper mapper,
    ITokenService tokenService,
    IUserService userService
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

    public async Task RegisterAsync(RegisterRequest dto)
    {
        // getting the mapped user from the DTO
        User newUser = mapper.Map<User>(dto);

        // checking if the username already exists, username should be unique
        if (await userRepository.GetByUsernameAsync(dto.username) != null)
        {
            throw new AlreadyExistsException("username already exists");
        }
        // checking if the Email already exists
        if (await userRepository.GetByEmail(dto.email) != null)
        {
            throw new AlreadyExistsException("Email already in use");
        }

        await userService.AddUserAsync(newUser);

        // create refresh token
        await tokenService.GenerateRefreshTokenAsync(newUser);
    }
}