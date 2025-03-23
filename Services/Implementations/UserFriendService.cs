using Microsoft.EntityFrameworkCore;
using URLshortner.Dtos.Implementations;
using URLshortner.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories.Implementations;
using URLshortner.Repositories.Interfaces;
using URLshortner.Services.Interfaces;

namespace URLshortner.Services.Implementations;

public class UserFriendService(
    IUserRepository userRepository,
    IUserFriendRepository friendRepository
    ) : IUserFriendService
{
    public async Task AddItemAsync(int Id, int FriendId)
    {
        if (await userRepository.GetByIdAsync(FriendId) == null)
        {
            throw new NotFoundException("The friend you want to add does not exist");
        }

        UserFriend friend = new UserFriend
        {
            UserId = Id,
            FriendId = FriendId
        };

        await friendRepository.AddAsync(friend);
    }

    public async Task DeleteItemAsync(int Id, int FriendId)
    {
        if (await userRepository.GetByIdAsync(FriendId) == null)
        {
            throw new NotFoundException("The friend you want to delete does not exist");
        }

        UserFriend friend = new UserFriend
        {
            UserId = Id,
            FriendId = FriendId
        };

        if (await friendRepository.ExistsAsync(friend) == false)
        {
            throw new NotFoundException("You don't have a friend with this ID");
        }

        await friendRepository.DeleteFriend(friend);
    }

    public async Task<PagedResult<string>> GetItemsPagedAsync(int userId, int pageNumber, int pageSize)
    {
        var result = await friendRepository.GetPagedAsync(userId, pageNumber, pageSize);
        return result;
    }
}