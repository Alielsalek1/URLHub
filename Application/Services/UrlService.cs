using Microsoft.AspNetCore.Http.HttpResults;
using URLshortner.Dtos;
using URLshortner.Domain.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Repositories.Interfaces;
using URLshortner.Services.Interfaces;
using URLshortner.Utils;

namespace URLshortner.Services;
public class UrlService(IUrlRepository UrlRepository) : IUrlService
{
    public async Task<PagedResult<string>> GetItemsPagedAsync(int userId, int pageNumber, int pageSize)
    {
        return await UrlRepository.GetPagedAsync(userId, pageNumber, pageSize);
    }

    public async Task AddItemAsync(UrlRequest dto, int userId)
    {
        try
        {
            var uri = new Uri(dto.url);
        }
        catch (UriFormatException ex)
        {
            throw new UriFormatException("The URL you entered isn't a valid URL");
        }

        if (dto.url.EndsWith("/"))
        {
            dto.url = dto.url.Remove(dto.url.Length - 1);
        }

        if (await UrlRepository.ExistsForUserAsync(userId, dto.url))
        {
            throw new AlreadyExistsException("The Url is already shortened");
        }

        string shortenedUrl = "";
        ulong additioner = 0;
        do
        {
            shortenedUrl = Helpers.ShortenUrl(dto.url, additioner);
            additioner++;
        } while (await UrlRepository.ExistsAsync(shortenedUrl));

        var mappedUrl = new MappedUrl
        {
            longUrl = dto.url,
            shortUrl = shortenedUrl,
        };

        await UrlRepository.AddAsync(userId, mappedUrl);
    }

    public async Task DeleteItemAsync(string shortUrl, int userId)
    {
        if (await UrlRepository.ExistsAsync(shortUrl) == false)
        {
            throw new NotFoundException($"no such Url exists");
        }
        if (await UrlRepository.ExistsForUserAsync(userId, shortUrl) == false)
        {
            throw new NotFoundException("User doesn't have this url");
        }

        await UrlRepository.DeleteFromUserAsync(userId, shortUrl);
    }

    public async Task<string> GetOriginalUrlAsync(string shortUrl)
    {
        var longUrl = await UrlRepository.GetLongUrlAsync(shortUrl);

        if (longUrl == null)
        {
            throw new NotFoundException("no such Url exists");
        }

        return longUrl;
    }
}