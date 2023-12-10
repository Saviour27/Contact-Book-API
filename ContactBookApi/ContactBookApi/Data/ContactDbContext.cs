using ContactBookApi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApi.Data
{
    public class ContactDbContext : IdentityDbContext<AppUser>
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
        {
            
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Contact>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.AppUserId)
                .IsRequired();

            base.OnModelCreating(builder);
        }
    }
}
