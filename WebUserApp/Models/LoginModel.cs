using System.ComponentModel.DataAnnotations;

namespace WebUserApp.Models
{
    public class LoginModel
    {
        
        [Required(ErrorMessage = "Email is requried!"),EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is requried!")]
        public string password { get; set; }

    }
}
