using Microsoft.AspNetCore.Identity;

namespace Q.Models
{
    public class User : IdentityUser
    {
        // Remove FullName and rely on UserName which is already defined in IdentityUser
        // public string FullName { get; set; }
    }
}