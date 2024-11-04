namespace URLshortner.Models;

public enum ROLE
{
    ADMIN,
    USER
}

public enum SEX
{
    MALE,
    FEMALE,
    DIKA
}

//TODO: make the usernames unique

public class User
{
    public int ID { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public ROLE Role { get; set; }
    public SEX Sex { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
