using BridgetSandalsAPI.Models;

namespace BridgetSandalsAPI.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        Task<bool> Login(User user);
        Task<bool> RegisterUser(User user);
    }
}