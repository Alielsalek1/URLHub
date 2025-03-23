﻿using URLshortner.Dtos.Implementations;
using URLshortner.Models;

namespace URLshortner.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User curUser);
    Task<RefreshToken> GenerateRefreshTokenAsync(User curUser);
    Task<string> RefreshAsync(TokenRefreshRequest dto);
    Task RenewRefreshTokenAsync(RefreshToken refreshToken);
    Task RevokeRefreshTokenAsync(User curUser);
}