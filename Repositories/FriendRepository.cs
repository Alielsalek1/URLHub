using ALL.Database;
using URLshortner.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLshortner.Repositories;

public class FriendRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<List<int>> GetFriendsById(int? id)
    {
        var friendPairs = await _context.Friends
            .Where(u => u.ID1 == id || u.ID2 == id)
            .ToListAsync();
        List<int> friends = new List<int>();
        foreach (var pair in friendPairs)
        {
            if (pair == null) continue;
            if (pair.ID1 == id) friends.Add((int)pair.ID2);
            else friends.Add((int)pair.ID1);
        }
        return friends;
    }

    public async Task AddFriend(Friend friend)
    {
        await _context.Friends.AddAsync(friend);
        await _context.SaveChangesAsync();
    }

    public async Task<Friend> GetFriend(Friend? friend)
    {
        var cur1 = await _context.Friends
                                   .FirstOrDefaultAsync(u => friend.ID1 == u.ID1 && friend.ID2 == u.ID2);
        var cur2 = await _context.Friends
                                   .FirstOrDefaultAsync(u => friend.ID1 == u.ID2 && friend.ID2 == u.ID1);
        return (cur1 ?? cur2);
    }

    public async Task RemoveFriend(Friend? friend)
    {
        var cur = await GetFriend(friend);
        if (cur != null)
        {
            _context.Friends.Remove(cur);
            await _context.SaveChangesAsync();
        }
    }
}
