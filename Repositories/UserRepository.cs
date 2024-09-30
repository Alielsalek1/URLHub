using ALL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using URLshortner.Models;

namespace URLshortner.Repositories;

public class UserRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<User> GetUserById(int? ID)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.ID == ID);
    }
    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    public async Task RemoveUser(int? ID)
    {
        var curUser = await GetUserById(ID);
        if (curUser != null)
        {
            _context.Users.Remove(curUser);
            await _context.SaveChangesAsync();
        }
    }
    public async Task UpdateUser(int? ID, User? user)
    {
        var curUser = await GetUserById(ID);
        if (curUser != null)
        {
            curUser = user;
            await _context.SaveChangesAsync();
        }
    }
}