using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Users
{
    public class AuthenticateRequest
    {
        [Required]
        public string Number { get; set; }

        [Required]
        public string Password { get; set; }
    }
}