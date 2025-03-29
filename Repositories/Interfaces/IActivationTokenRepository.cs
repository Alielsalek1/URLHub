using URLshortner.Models;

namespace URLshortner.Repositories.Interfaces;

public interface IActivationTokenRepository
{
    Task AddAsync(ActivationToken activationToken);
    Task DeleteAsync(ActivationToken activationToken);
    Task<ActivationToken> GetByUserIdAsync(int userId, string token);
}