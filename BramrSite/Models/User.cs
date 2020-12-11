namespace BramrSite.Models
{
    public class User
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsValidSignUp() => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(UserName);

        public bool IsValidSignIn() => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
    }
}
