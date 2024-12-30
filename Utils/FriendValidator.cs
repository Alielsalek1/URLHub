using URLshortner.Models;

namespace URLshortner.Services;

public class FriendValidator
{
    public bool IsValidFriend(Friend? friend)
    {
        return IsValidID(friend.ID) &&
            IsValidID(friend.FriendID) &&
            friend.ID != friend.FriendID;
    }
    public bool IsValidID(int? ID)
    {
        return ID != null && ID >= 1;
    }
}
