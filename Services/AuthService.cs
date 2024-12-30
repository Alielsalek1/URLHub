using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using URLshortner.Dtos;
using URLshortner.Exceptions;
using URLshortner.Models;
using URLshortner.Repositories;

namespace URLshortner.Services;

public class AuthService(UserRepository userRepository, TokenGenerator tokenGenerator)
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly TokenGenerator _tokenGenerator = tokenGenerator;

    public async Task<string> Login(AuthRequestDto dto)
    {
        var curUser = await _userRepository.GetUserByUsername(dto.username);
        if (curUser == null || curUser.Password != dto.password)
        {
            throw new InvalidUserException("Invalid Credentials");
        }

        var token = _tokenGenerator.GenerateJwtToken(curUser);

        Console.WriteLine($"Generated Token: {token}");
        return token;
    }
    
    public async Task<string> Register(AuthRequestDto dto)
    {
        if (await _userRepository.GetUserByUsername(dto.username) != null)
        {
            throw new UserAlreadyExists("username already exists");
        }

        var newUser = new User
        {
            Username = dto.username,
            Password = dto.password
        };

        await _userRepository.AddUser(newUser);
        
        return "User Created Successfully";
    }
}