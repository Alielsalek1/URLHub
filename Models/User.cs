using static System.Runtime.InteropServices.JavaScript.JSType;

namespace URLshortner.Models
{

    public class User
    {
        public enum ROLE
        {
            ADMIN = 0,
            USER
        }
        public enum SEX
        {
            MALE = 0,
            FEMALE,
            DIKA
        }

        private int _ID;
        private string? _Username;
        private string? _Password;
        private string? _Email;
        private ROLE _Role;
        private SEX _Sex;
        private DateTime? _DateOfBirth;

        public int ID { get { return _ID; } set { _ID = value; } }
        public string? Username { get { return _Username; } set { _Username = value; } }
        public string? Password { get { return _Password; } set { _Password = value; } }
        public string? Email { get { return _Email; } set { _Email = value; } }
        public ROLE Role { get { return _Role; } set { _Role = value; } }
        public SEX Sex { get { return _Sex; } set { _Sex = value; } }
        public DateTime? DateOfBirth { get { return _DateOfBirth; } set { _DateOfBirth = value; } }
    }
}
