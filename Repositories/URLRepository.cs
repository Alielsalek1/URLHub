using ALL.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using URLshortner.Models;

namespace URLshortner.Repositories;

public class URLRepository(AppDbContext context)
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
    public async Task<URL> GetInstance(URL? url)
    {
        return await _context.URLs.FirstOrDefaultAsync(u => u.ID == url.ID && u.Url == url.Url);
    }
    public async Task RemoveURL(URL? url)
    {
        var cur = await GetInstance(url);
        if (cur != null)
        {
            _context.URLs.Remove(url);
            await _context.SaveChangesAsync();
        }
    }
}
