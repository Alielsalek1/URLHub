using URLshortner.Models;

namespace URLshortner.Repositories.Interfaces;

public interface IActionTokenRepository
{
    Task AddAsync(string tokenKey, string token);
    Task<string> GetToken(string tokenKey);
}