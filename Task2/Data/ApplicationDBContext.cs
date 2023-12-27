using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<InventoryLeadsEntity> InventoryUsers { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
