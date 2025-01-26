using WealthFlow.Application.Security.Interfaces;
using BCrypt.Net;

namespace WealthFlow.Application.Security.Services
{
    public class PassowordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
