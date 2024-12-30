using Microsoft.AspNetCore.Mvc;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Services;

namespace URLshortner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class friendController(FriendRepository repository, UserRepository useRepository, FriendValidator validator) : ControllerBase
{
    private FriendValidator _validator = validator;
    private UserRepository _userRepository = useRepository;
    private FriendRepository _repository = repository;

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<string>> AddFriend([FromBody] Friend? friend)
    {
        if (!ModelState.IsValid || !_validator.IsValidFriend(friend))
        {
            return BadRequest("Invalid Credentials");
        }

        var IsUser1Found = await _userRepository.GetUserById(friend.ID);
        var IsUser2Found = await _userRepository.GetUserById(friend.FriendID);

        if (IsUser1Found == null)
        {
            return NotFound($"User with ID {friend.ID} isn't registered");
        }
        if (IsUser2Found == null)
        {
            return NotFound($"User with ID {friend.FriendID} isn't registered");
        }

        var IsFriends = await _repository.GetFriend(friend);

        if (IsFriends != null)
        {
            return NotFound($"Users with IDs {friend.ID} and {friend.FriendID} are already friends");
        }

        await _repository.AddFriend(friend);

        return Ok($"Friends for users {friend.ID} & {friend.FriendID} were added successfully");
    }
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<List<int>>> GetMyFriends(int? id)
    {
        if (!ModelState.IsValid || !_validator.IsValidID(id))
        {
            return BadRequest("Invalid Credentials");
        }

        var IsUserFound = await _userRepository.GetUserById(id);

        if (IsUserFound == null)
        {
            return NotFound($"User with ID {id} isn't registered");
        }

        var MyFriends = await _repository.GetFriendsById(id);

        return Ok(new { Message = $"Friends for user {id} retrieved successfully!", friends = MyFriends });
    }
    [HttpDelete]
    [Route("")]
    public async Task<ActionResult<string>> DeleteFriend(Friend? friend)
    {
        if (!ModelState.IsValid || !_validator.IsValidFriend(friend))
        {
            return BadRequest("Invalid Credentials");
        }

        var IsUser1Found = await _userRepository.GetUserById(friend.ID);
        var IsUser2Found = await _userRepository.GetUserById(friend.FriendID);

        if (IsUser1Found == null)
        {
            return NotFound($"User with ID {friend.ID} isn't registered");
        }
        if (IsUser2Found == null)
        {
            return NotFound($"User with ID {friend.FriendID} isn't registered");
        }

        var IsFriends = await _repository.GetFriend(friend);

        if (IsFriends == null)
        {
            return NotFound($"Users with IDs {friend.ID} and {friend.FriendID} are not even friends");
        }

        await _repository.RemoveFriend(friend);

        return Ok($"Users with IDs {friend.ID} and {friend.FriendID} are not friends anymore");
    }
}