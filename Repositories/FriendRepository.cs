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
            .Where(u => u.ID == id || u.FriendID == id)
            .ToListAsync();
        List<int> friends = new List<int>();
        foreach (var pair in friendPairs)
        {
            if (pair == null) continue;
            if (pair.ID == id) friends.Add((int)pair.FriendID);
            else friends.Add((int)pair.ID);
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
                                   .FirstOrDefaultAsync(u => friend.ID == u.ID && friend.FriendID == u.FriendID);
        var cur2 = await _context.Friends
                                   .FirstOrDefaultAsync(u => friend.ID == u.FriendID && friend.FriendID == u.ID);
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
