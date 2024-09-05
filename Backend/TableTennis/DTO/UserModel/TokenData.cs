namespace DTO.UserModel
{
    public class TokenData
    {
        public Guid UserId { get; set; } // Usklađeno s bazom podataka
        public string Username { get; set; } // Usklađeno s bazom podataka
        public string Email { get; set; }
        public string Role { get; set; } // Ovo polje može biti string za naziv role, ako se koristi
        public string Token { get; set; } // Token za autentifikaciju ako postoji
    }
}
