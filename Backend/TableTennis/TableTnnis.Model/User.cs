using System;

namespace TableTennis.Model
{
    public class User
    {
        public Guid UserId { get; set; } // Koristi se user_id iz baze
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedAt { get; set; } // Nullable jer TIMESTAMP može biti NULL
        public Guid? RoleId { get; set; } // Nullable jer je stranjski ključ postavljen na NULL ON DELETE
    }
}
