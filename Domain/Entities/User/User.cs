﻿namespace WealthFlow.Domain.Entities.User
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; }
        public string Role { get; set; } = "User";
        public string RecoveryEmail { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
