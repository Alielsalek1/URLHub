using Microsoft.AspNetCore.Mvc;
using URLshortner.Models;
using URLshortner.Repositories;
using URLshortner.Validators;

namespace URLshortner.Controllers;

[ApiController]
[Route("[controller]")]
public class friendController(FriendRepository repository, UserRepository useRepository, FriendValidator validator) : ControllerBase
{
    public FriendValidator _validator = validator;
    public UserRepository _userRepository = useRepository;
    public FriendRepository _repository = repository;

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<string>> AddFriend([FromBody] Friend? friend)
    {
        if (!ModelState.IsValid || !_validator.IsValidFriend(friend))
        {
            return BadRequest("Invalid Credentials");
        }

        var IsUser1Found = await _userRepository.GetUserById(friend.ID1);
        var IsUser2Found = await _userRepository.GetUserById(friend.ID2);

        if (IsUser1Found == null)
        {
            return NotFound($"User with ID {friend.ID1} isn't registered");
        }
        if (IsUser2Found == null)
        {
            return NotFound($"User with ID {friend.ID2} isn't registered");
        }

        var IsFriends = await _repository.GetFriend(friend);

        if (IsFriends != null)
        {
            return NotFound($"Users with IDs {friend.ID1} and {friend.ID2} are already friends");
        }

        await _repository.AddFriend(friend);

        return Ok($"Friends for users {friend.ID1} & {friend.ID2} were added successfully");
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

        var IsUser1Found = await _userRepository.GetUserById(friend.ID1);
        var IsUser2Found = await _userRepository.GetUserById(friend.ID2);

        if (IsUser1Found == null)
        {
            return NotFound($"User with ID {friend.ID1} isn't registered");
        }
        if (IsUser2Found == null)
        {
            return NotFound($"User with ID {friend.ID2} isn't registered");
        }

        var IsFriends = await _repository.GetFriend(friend);

        if (IsFriends == null)
        {
            return NotFound($"Users with IDs {friend.ID1} and {friend.ID2} are not even friends");
        }

        await _repository.RemoveFriend(friend);

        return Ok($"Users with IDs {friend.ID1} and {friend.ID2} are not friends anymore");
    }
}