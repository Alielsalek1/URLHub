using URLshortner.Dtos;
using URLshortner.Models;

namespace URLshortner.Repositories.Interfaces;

public interface IUserFriendRepository
{
    Task<PagedResult<string>> GetPagedAsync(int userId, int pageNumber, int pageSize);
    Task<bool> ExistsAsync(UserFriend item);
    Task AddAsync(UserFriend item);
    Task DeleteFriend(UserFriend item);
}