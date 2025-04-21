using Microsoft.AspNetCore.Identity;

namespace ProkodersECommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }
    }
}
