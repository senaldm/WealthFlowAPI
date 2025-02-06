namespace WealthFlow.Application.Users.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string RecoveryEmail { get; set; } = string.Empty;
    }
}
