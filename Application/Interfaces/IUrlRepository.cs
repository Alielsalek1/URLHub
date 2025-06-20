using URLshortner.Dtos;
using URLshortner.Models;

namespace URLshortner.Repositories.Interfaces;

public interface IUrlRepository
{
    Task<PagedResult<string>> GetPagedAsync(int userId, int pageNumber, int pageSize);
    Task<bool> ExistsAsync(string shortUrl);
    Task<bool> ExistsForUserAsync(int UserId, string longUrl);
    Task AddAsync(int UserId, MappedUrl item);
    Task<string> GetShortUrlAsync(string longUrl);
    Task<string> GetLongUrlAsync(string shortUrl);
    Task DeleteFromUserAsync(int UserId, string shortUrl);
}