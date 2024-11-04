using URLshortner.Models;

namespace URLshortner.Services;

public class FriendValidator
{
    public bool IsValidFriend(Friend? friend)
    {
        return IsValidID(friend.ID1) &&
            IsValidID(friend.ID2) &&
            friend.ID1 != friend.ID2;
    }
    public bool IsValidID(int? ID)
    {
        return ID != null && ID >= 1;
    }
}
