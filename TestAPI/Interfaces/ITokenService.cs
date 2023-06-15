using TestAPI.Models;

namespace TestAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
