using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.UserModel
{
    public class RegisterPost
    {
        [Required]
        public string Username { get; set; }        // Korisničko ime

        [Required]
        [EmailAddress]
        public string Email { get; set; }           // Email korisnika

        [Required]
        [MinLength(6)]
        public string Password { get; set; }        // Lozinka korisnika

        [Required]
        public Guid RoleId { get; set; }            // ID uloge korisnika (npr. admin, user)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Datum kreiranja (postavlja se automatski)
    }
}
