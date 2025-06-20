using ALL.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using URLshortner.Dtos;
using URLshortner.Models;
using URLshortner.Repositories.Interfaces;

namespace URLshortner.Repositories;

// optimize be Raw Sql queries & ExecuteAsync
public class UrlRepository(AppDbContext context) : IUrlRepository
{
    public async Task AddAsync(int userId, MappedUrl item)
    {
        var existingUrl = await context.MappedUrls.FirstOrDefaultAsync(mu => mu.shortUrl == item.shortUrl);

        // eager loading
        var user = await context.Users.Include(u => u.MappedUrls).FirstOrDefaultAsync(u => u.Id == userId);

        if (existingUrl == null)
        {
            await context.MappedUrls.AddAsync(item);
            user.MappedUrls.Add(item); // adding new instance to urls and linking it to the user
        } else
        {
            user.MappedUrls.Add(existingUrl); // only linking it to the user
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteFromUserAsync(int UserId, string shortUrl)
    {
        // Retrieve the user along with their associated MappedUrls as eager loading.
        var user = await context.Users
            .Include(u => u.MappedUrls)
            .FirstOrDefaultAsync(u => u.Id == UserId);

        // Find the mapped URL in the user's collection.
        var mappedUrl = user.MappedUrls.FirstOrDefault(mu => mu.shortUrl == shortUrl);

        // Remove the mapped URL from the user's collection.
        user.MappedUrls.Remove(mappedUrl);

        // When SaveChangesAsync is called, EF Core will update the join table accordingly,
        // removing the association without deleting the mappedUrl record.
        await context.SaveChangesAsync();
    }

    public async Task<PagedResult<string>> GetPagedAsync(int userId, int pageNumber, int pageSize)
    {
        var query = context.MappedUrls
            .Where(mu => mu.Users.Any(u => u.Id == userId))
            .Select(u => u.shortUrl);

        var totalRecords = await query.CountAsync();

        var mappedUrls = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<string>
        {
            Items = mappedUrls,
            TotalCount = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<bool> ExistsAsync(string shortUrl)
    {
        return await context.MappedUrls.AnyAsync(mu => shortUrl == mu.shortUrl);
    }

    public async Task<bool> ExistsForUserAsync(int UserId, string longUrl)
    {
        return await context.MappedUrls.
            AnyAsync(mu => mu.longUrl == longUrl && mu.Users.Any(u => u.Id == UserId));
    }

    public async Task<string> GetShortUrlAsync(string longUrl)
    {
        var mappedUrl = await context.MappedUrls.FirstOrDefaultAsync(u => u.longUrl == longUrl);
        return mappedUrl?.shortUrl;
    }

    public async Task<string> GetLongUrlAsync(string shortUrl)
    {
        var mappedUrl = await context.MappedUrls.FindAsync(shortUrl);
        return mappedUrl?.longUrl;
    }
}
