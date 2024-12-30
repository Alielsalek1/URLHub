using URLshortner.Dtos;
using URLshortner.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories;

namespace URLshortner.Services;

public class UserService(UserRepository userRepository)
{
    private readonly UserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    public async Task<UserResponseDTO> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id) 
                   ?? throw new KeyNotFoundException($"User with id {id} was not found");

        return new UserResponseDTO
        {
            username = user.Username,
            id = user.ID
        };
    }

    public async Task UpdateUser(int id, UpdateUserRequestDto dto)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new Exception($"User with id {id} was not found");
        }

        var isUsernameUsed = await _userRepository.GetUserByUsername(dto.username);
        if (isUsernameUsed != null)
        {
            throw new UsernameUsed("This username is already taken");
        }

        var updatedUser = new User
        {
            Username = dto.username ?? user.Username,
            Password = dto.password ?? user.Password
        };

        await _userRepository.UpdateUser(id, updatedUser);
    }
}