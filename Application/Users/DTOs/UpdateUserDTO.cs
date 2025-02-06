﻿namespace WealthFlow.Application.Users.DTOs
{
    public class UpdateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string RecoveryEmail { get; set; } = string.Empty;

    }
}
