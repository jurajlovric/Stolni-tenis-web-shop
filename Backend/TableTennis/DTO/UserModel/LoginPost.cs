using System.ComponentModel.DataAnnotations;

namespace DTO.UserModel
{
    public class LoginPost
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
