using System;

namespace DTO.UserModel
{
    public class TokenData
    {
        public Guid Id { get; set; }               // ID korisnika
        public string Email { get; set; }          // Email korisnika
        public string FirstName { get; set; }      // Ime korisnika
        public string LastName { get; set; }       // Prezime korisnika
        public string Role { get; set; }           // Ime uloge korisnika (npr. Admin, User)
    }
}
