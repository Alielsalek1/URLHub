using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using URLshortner.Models;
using URLshortner.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using URLshortner.Services;

namespace URLshortner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(TokenGenerator tokenGenerator, UserRepository repository) : ControllerBase
{
    private readonly UserRepository _repository = repository;
    private readonly TokenGenerator _tokenGenerator = tokenGenerator;

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> login(string Username, string Password)
    {
        var curUser = await _repository.GetUserByUsername(Username);
        if (curUser == null || curUser.Password != Password)
        {
            return BadRequest("Invalid Credentials");
        }

        var token = _tokenGenerator.GenerateJwtToken(curUser);

        Console.WriteLine($"Generated Token: {token}");

        return Ok(token);
    }

}
