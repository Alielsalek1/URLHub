using ALL.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Validators;

namespace URLshortner.Controllers;

[ApiController]
[Route("[controller]")]
public class userController(UserRepository repository, UserValidator validator) : ControllerBase
{
    private readonly UserRepository _repository = repository;
    private readonly UserValidator _validator = validator;
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<string>> AddUser([FromBody] User? user)
    {
        if (!ModelState.IsValid || !_validator.IsValidUser(user))
        {
            return BadRequest("Invalid Credentials");
        }

        user.ID = 0;
        await _repository.AddUser(user);

        return Ok(($"User with ID {user.ID} was added successfully."));
    }
    [HttpDelete]
    [Route("{ID}")]
    public async Task<ActionResult<string>> DeleteUser(int? ID)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid Credentials.");
        }

        var IsUserFound = await _repository.GetUserById(ID);

        if (IsUserFound == null)
        {
            return NotFound($"User with ID {ID} not found.");
        }

        await _repository.RemoveUser(ID);

        return Ok(($"User with ID {ID} was Deleted successfully."));
    }
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<User>> GetUser(int? id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid Credentials");
        }

        User? user = await _repository.GetUserById(id);

        if (user == null)
        {
            return NotFound($"User with ID {id} nof found");
        }

        return Ok(new {
            Message = $"User with ID {id} retrieved successfully.",
            User = user 
        });
    }
    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<string>> UpdateUser(int? id, [FromBody] User? NewUser)
    {
        if (!ModelState.IsValid || !_validator.IsValidUser(NewUser))
        {
            return BadRequest("Invalid Credentials");
        }

        var IsUserFound = await _repository.GetUserById(id);

        if (IsUserFound == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        await _repository.UpdateUser(id, NewUser);

        return Ok($"User with ID {id} updated Successfully.");
    }
}
