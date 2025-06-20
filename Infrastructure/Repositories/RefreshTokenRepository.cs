using ALL.Database;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.Marshalling;
using URLshortner.Models;
using URLshortner.Repositories.Interfaces;

namespace URLshortner.Repositories;

public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken item)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == item.UserId);
        user.RefreshToken = item;
        await context.SaveChangesAsync();
    }

    public async Task<RefreshToken> GetRefreshTokenByUserIdAsync(int userId)
    {
        var user = await context.Users.
            AsNoTracking().
            Include(u => u.RefreshToken).
            FirstOrDefaultAsync(u => u.Id == userId);

        return user?.RefreshToken;
    }

    public async Task UpdateAsync(RefreshToken item)
    {
        var user = await context.Users.
            Include(u => u.RefreshToken).
            FirstOrDefaultAsync(u => u.Id == item.UserId);

        user.RefreshToken = item;
        await context.SaveChangesAsync();
    }
}