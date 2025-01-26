using WealthFlow.Application.Users.DTOs;

namespace WealthFlow.Application.Security.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(UserDTO userDto);
        string GetJwtToken();
    }
}
