using ALL.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using URLshortner.Models;

namespace URLshortner.Repositories;

public class UrlRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;
    public async Task<List<URL>> GetURLsById(int? id)
    {
        return await _context.URLs.Where(u => u.ID == id).ToListAsync();
    }
    public async Task AddURL(URL url)
    {
        await _context.URLs.AddAsync(url);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> exists(URL url)
    {
        var Durl = await _context.URLs.FirstOrDefaultAsync(u => u.ID == url.ID && u.Url == url.Url);

        return (Durl != null);
    }
    public async Task RemoveURL(URL url)
    {
        if (url == null) return;

        if (await exists(url))
        {
            _context.URLs.Remove(_context.URLs.FirstOrDefault(u => u.ID == url.ID && u.Url == url.Url));
            await _context.SaveChangesAsync();
        }
    }
}
