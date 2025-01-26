namespace WealthFlow.Application.Users.DTOs
{
    public class UserRegistrationDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string RecoveryEmail { get; set; } = string.Empty;




    }
}
