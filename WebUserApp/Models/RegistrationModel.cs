using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebUserApp.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Username is requried!")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is requried!")]
        public string password { get; set; }
        
        [Required(ErrorMessage = "Email is requried!"), EmailAddress]
        public string? email { get; set; }
        
        public string? firstName { get; set; }
        public string? lastName { get; set; }
    }
}
