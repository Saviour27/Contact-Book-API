using Microsoft.AspNetCore.Identity;

namespace ContactBookApi.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}
