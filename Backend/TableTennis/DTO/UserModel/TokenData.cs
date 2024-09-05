namespace DTO.UserModel
{
    public class TokenData
    {
        public Guid UserId { get; set; } 
        public string Username { get; set; } 
        public string Email { get; set; }
        public string Role { get; set; } 
        public string Token { get; set; }
    }
}
