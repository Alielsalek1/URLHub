using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Services;

namespace URLshortner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class urlController(URLRepository repository, UserRepository userRepository) : ControllerBase
{
    private readonly URLRepository _repository = repository;
    private readonly UserRepository _userRepository = userRepository;

    [HttpGet]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> GetMyURLs(int? id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid Credentials");
        }

        List<URL> urls = await _repository.GetURLsById(id);

        return Ok(new {
            Message = $"URLs for user with ID {id} have been retrieved successfully",
            URLs = urls
        });
    }

    // TODO : Make GetFriendsURLs (Pagination)

    [HttpPost]
    [Route("")]
    [Authorize]
    public async Task<IActionResult> AddURL([FromBody] URL url)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid Credentials");
        }

        // TODO : Same Urls but different paths (https://www.facebook.com/ & https://www.facebook.com)

        var IsUserPresent = await _userRepository.GetUserById(url.ID);

        if (IsUserPresent == null)
        {
            return NotFound($"User with ID {url.ID} isn't registered");
        }

        await _repository.AddURL(url);

        return Ok($"URL for user {url.ID} has been added successfully");
    }

    [HttpDelete]
    [Route("")]
    [Authorize]
    public async Task<IActionResult> RemoveURL([FromBody] URL? url)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid Credentials");
        }

        var IsPresent = await _repository.GetInstance(url);

        if (IsPresent == null)
        {
            return NotFound($"URL for user with ID {url.ID} not found");
        }

        await _repository.RemoveURL(url);

        return Ok($"URL for user with ID {url.ID} was deleted successfully");
    }
}
