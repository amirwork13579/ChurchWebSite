using Church4Site.Entities;
using Church4Site.Models;

namespace Church4Site.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<string> LoginAsync(UserDto request);
    }
}
