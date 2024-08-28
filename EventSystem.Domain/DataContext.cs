using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventSystem.Domain
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        protected readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(_configuration.GetConnectionString("EventDBString"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            );

            // Configure Event entity with auto-increment
            builder.Entity<Event>()
                .Property(e => e.Id) 
                .ValueGeneratedOnAdd();

            builder.Entity<EventRegistration>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<IdentityUserClaim<string>> IdentityUserClaim { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventRegistration> EventRegistration { get; set; }
    }
}
