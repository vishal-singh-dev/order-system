using System.ComponentModel.DataAnnotations;

namespace auth_service.Models.DTOs
{
    public class Login
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
