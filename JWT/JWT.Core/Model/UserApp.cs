using Microsoft.AspNetCore.Identity;

namespace JWT.Core.Model
{
    public class UserApp : IdentityUser
    {
        public string? City { get; set; }
    }
}
