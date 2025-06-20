using URLshortner.Dtos;

namespace URLshortner.Services.Interfaces;

public interface IUrlService
{
    Task AddItemAsync(UrlRequest dto, int userId);
    Task DeleteItemAsync(string shortUrl, int userId);
    Task<PagedResult<string>> GetItemsPagedAsync(int userId, int pageNumber, int pageSize);
    Task<string> GetOriginalUrlAsync(string shortUrl);
}
