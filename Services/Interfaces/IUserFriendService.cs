using URLshortner.Dtos.Implementations;

namespace URLshortner.Services.Interfaces;

public interface IUserFriendService
{
    Task AddItemAsync(int Id, int FriendId);
    Task DeleteItemAsync(int Id, int FriendId);
    Task<PagedResult<string>> GetItemsPagedAsync(int userId, int pageNumber, int pageSize);
}