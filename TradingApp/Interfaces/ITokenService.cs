using TradingApp.Entities;

namespace TradingApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}