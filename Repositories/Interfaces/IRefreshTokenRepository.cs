using URLshortner.Models;

namespace URLshortner.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken item);
    Task UpdateAsync(RefreshToken item);
    Task<RefreshToken> GetRefreshTokenByUserIdAsync(int userId);
}
