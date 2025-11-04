using FINANCE.TRACKER.API.Models.UserManager;

namespace FINANCE.TRACKER.API.Services.Interfaces.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(UserModel user);
    }
}
