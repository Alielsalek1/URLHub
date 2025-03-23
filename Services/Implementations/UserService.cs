using AutoMapper;
using Microsoft.AspNetCore.Identity;
using URLshortner.Dtos.Implementations;
using URLshortner.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories.Implementations;
using URLshortner.Repositories.Interfaces;
using URLshortner.Services.Interfaces;

namespace URLshortner.Services.Implementations;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<UserResponse> GetUserByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);

        var response = mapper.Map<UserResponse>(user);
        return response;
    }

    public async Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest dto)
    {
        var user = await userRepository.GetByIdAsync(id);

        var isUsernameUsed = await userRepository.GetByUsernameAsync(dto.username);
        if (isUsernameUsed != null && isUsernameUsed.Id != user.Id)
        {
            throw new AlreadyExistsException("This username is already taken");
        }

        if (dto.username != null)
        {
            user.Username = dto.username;
        }
        if (dto.password != null)
        {
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(null, dto.password);
        }
        
        await userRepository.UpdateAsync(user);

        var response = mapper.Map<UserResponse>(user);
        return response;
    }

    public async Task AddUserAsync(User user)
    {
        // hashing the user password
        var passwordHasher = new PasswordHasher<string>();
        user.Password = passwordHasher.HashPassword(null, user.Password);

        // adding the user to the database
        await userRepository.AddAsync(user);
    }
}