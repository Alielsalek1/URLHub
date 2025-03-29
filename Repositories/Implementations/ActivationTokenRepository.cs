using ALL.Database;
using Microsoft.EntityFrameworkCore;
using URLshortner.Models;
using URLshortner.Repositories.Interfaces;

namespace URLshortner.Repositories.Implementations;

public class ActivationTokenRepository(AppDbContext context) : IActivationTokenRepository
{
    public async Task AddAsync(ActivationToken activationToken)
    {
        var user = await context.Users.
            Include(u => u.ActivationTokens).
            FirstOrDefaultAsync(u => u.Id == activationToken.UserId);

        user.ActivationTokens.Add(activationToken);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ActivationToken activationToken)
    {
        var user = await context.Users.
            Include(u => u.ActivationTokens).
            FirstOrDefaultAsync(u => u.Id == activationToken.UserId);
        user.ActivationTokens.Remove(activationToken);
        await context.SaveChangesAsync();
    }

    public async Task<ActivationToken> GetByUserIdAsync(int userId, string token)
    {
        var user = await context.Users.
            Include(u => u.ActivationTokens).
            FirstOrDefaultAsync(u => u.Id == userId);

        if (!Guid.TryParse(token, out Guid tokenGuid)) 
        {
            return null;
        }

        return user.ActivationTokens.FirstOrDefault(at => at.Token == tokenGuid);
    }
}
