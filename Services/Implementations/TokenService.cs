using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using URLshortner.Dtos.Implementations;
using URLshortner.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories.Implementations;
using URLshortner.Repositories.Interfaces;
using URLshortner.Services.Interfaces;

namespace URLshortner.Services.Implementations;

public class TokenService(
    IConfiguration configuration,
    IRefreshTokenRepository tokenRepository,
    IUserRepository userRepository
    ) : ITokenService
{
    public string GenerateJwtToken(User curUser)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, curUser.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var secretKey = configuration["JwtSettings:Key"];
        var issuer = configuration["JwtSettings:Issuer"];
        var audience = configuration["JwtSettings:Audience"];

        // getting the security key
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        // hashing the credentials using HmacSha256 hashing algorithm
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var TokenLifetime = configuration.GetValue<int>("JwtSettings:AccessDurationInMinutes");

        // crafting the token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(TokenLifetime),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(User curUser)
    {
        var TokenLifetime = configuration.GetValue<int>("JwtSettings:RefreshdurationInDays");
        var token = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = curUser.Id,
            Expires = DateTime.UtcNow.AddDays(TokenLifetime),
            User = curUser
        };

        await tokenRepository.AddAsync(token);

        return token;
    }

    public async Task RenewRefreshTokenAsync(RefreshToken refreshToken)
    {
        var TokenLifetime = configuration.GetValue<int>("JwtSettings:RefreshdurationInDays");
        refreshToken.Expires = DateTime.UtcNow.AddDays(TokenLifetime);
        await tokenRepository.UpdateAsync(refreshToken);
    }

    public async Task<string> RefreshAsync(TokenRefreshRequest dto)
    {
        var refreshToken = await tokenRepository.GetRefreshTokenByUserIdAsync(dto.userId);

        if (refreshToken == null)
        {
            throw new NotFoundException("No refresh token found");
        }
        if (refreshToken.IsRevoked || refreshToken.Token != dto.token || refreshToken.Expires < DateTime.UtcNow)
        {
            throw new UnAuthorizedException("user is not authorized to do this action");
        }

        var user = await userRepository.GetByIdAsync(dto.userId);
        var accessToken = GenerateJwtToken(user);

        return accessToken;
    }

    public Task RevokeRefreshTokenAsync(User curUser)
    {
        throw new NotImplementedException();
    }
}