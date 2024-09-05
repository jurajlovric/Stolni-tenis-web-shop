﻿using System;

namespace TableTennis.Model
{
    public class User
    {
        public Guid UserId { get; set; } 
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public Guid? RoleId { get; set; }
    }
}
