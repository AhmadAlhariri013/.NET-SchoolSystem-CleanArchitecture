using Microsoft.AspNetCore.Identity;

namespace SchoolProject.Data.Entities.Identities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
    }
}
