using Microsoft.AspNetCore.Identity;

namespace TradingApp.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public bool IsActive { get; set; } = true;
        public ICollection<AppUserRole> UserRoles { get; set; } = [];   
        public required string ToTPSecret { get; set; }     
    }
}