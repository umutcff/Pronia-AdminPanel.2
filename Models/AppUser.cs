using Microsoft.AspNetCore.Identity;

namespace ProniaUmut.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; } = null!;
    }
}
