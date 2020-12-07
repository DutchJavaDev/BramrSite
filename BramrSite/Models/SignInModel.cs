using System.ComponentModel.DataAnnotations;

namespace BramrSite.Models
{
    public class SigninModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is requird")]
        public string Password { get; set; }


        public void ResetModel()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }
    }
}
