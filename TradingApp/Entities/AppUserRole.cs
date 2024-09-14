

using Microsoft.AspNetCore.Identity;

namespace TradingApp.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public required AppUser User { get; set; }
        public required AppRole Role { get; set; }
    }
}