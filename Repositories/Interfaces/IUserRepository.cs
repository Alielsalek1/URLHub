using URLshortner.Models;

namespace URLshortner.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByIdAsync(int id);
    Task<User> GetByEmail(string email);
    Task AddAsync(User item);
    Task UpdateAsync(User item);
    Task<bool> ExistsAsync(User item);
}
