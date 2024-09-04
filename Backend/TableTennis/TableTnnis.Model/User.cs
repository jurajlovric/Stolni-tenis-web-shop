using System;

namespace TableTennis.Model
{
    public class User
    {
        public Guid UserId { get; set; }               // Jedinstveni identifikator korisnika
        public string Username { get; set; }           // Korisničko ime
        public string Email { get; set; }              // Email korisnika
        public string Password { get; set; }           // Hashirana lozinka
        public Guid RoleId { get; set; }               // ID uloge korisnika (veza na Role tablicu)
        public DateTime CreatedAt { get; set; }        // Datum kreiranja korisnika
    }
}
