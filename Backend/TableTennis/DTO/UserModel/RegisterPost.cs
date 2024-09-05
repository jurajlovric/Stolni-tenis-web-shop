using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.UserModel
{
    public class RegisterPost
    {
        [Required]
        public string Username { get; set; }       

        [Required]
        [EmailAddress]
        public string Email { get; set; } 

        [Required]
        [MinLength(6)]
        public string Password { get; set; } 

        [Required]
        public Guid RoleId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
