using ALL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using URLshortner.Models;
using URLshortner.Repositories.Interfaces;

namespace URLshortner.Repositories.Implementations;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User> GetByIdAsync(int Id)
    {
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
    }

    public async Task<User> GetByUsernameAsync(string Username)
    {
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == Username);
    }

    public async Task<User> GetByEmailAsync(string Email)
    {
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == Email);
    }

    public async Task AddAsync(User item)
    {
        await context.Users.AddAsync(item);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(User user)
    {
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        existingUser.Username = user.Username;
        existingUser.Password = user.Password;
        existingUser.IsEmailVerified = user.IsEmailVerified;

        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(User item)
    {
        return await context.Users.AnyAsync(u => u == item);
    }
}