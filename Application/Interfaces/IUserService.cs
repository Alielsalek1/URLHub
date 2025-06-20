using URLshortner.Dtos;
using URLshortner.Models;

namespace URLshortner.Services.Interfaces;

public interface IUserService
{
    Task<UserResponse> GetUserByIdAsync(int id);
    Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest dto);
    Task AddUserAsync(User user);
}
