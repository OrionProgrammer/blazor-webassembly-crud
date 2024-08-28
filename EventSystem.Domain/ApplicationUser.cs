using Microsoft.AspNetCore.Identity;

namespace EventSystem.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
