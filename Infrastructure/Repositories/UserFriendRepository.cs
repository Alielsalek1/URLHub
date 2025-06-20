using ALL.Database;
using URLshortner.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLshortner.Repositories.Interfaces;
using URLshortner.Dtos;

namespace URLshortner.Repositories;

public class UserFriendRepository(AppDbContext context) : IUserFriendRepository
{
    public async Task AddAsync(UserFriend item)
    {
        context.UserFriends.Add(item);
        await context.SaveChangesAsync();
    }

    public async Task DeleteFriend(UserFriend userFriend)
    {
        var relationship = await context.UserFriends.
            FirstOrDefaultAsync(u => u == userFriend);

        context.UserFriends.Remove(relationship);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(UserFriend item)
    {
        return await context.UserFriends
            .AnyAsync(uf => uf.UserId == item.UserId && uf.FriendId == item.FriendId);
    }

    public async Task<PagedResult<string>> GetPagedAsync(int userId, int pageNumber, int pageSize)
    {
        // Query only the relationships where the given user is the initiator.
        var query = context.UserFriends
            .Where(uf => uf.UserId == userId)
            .Select(uf => uf.Friend.Username);

        var totalRecords = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<string>
        {
            Items = items,
            TotalCount = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
