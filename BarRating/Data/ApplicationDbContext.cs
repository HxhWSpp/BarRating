using BarRating.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BarRating.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "c6384989-53c7-4a77-b0d2-76e7371b4e2a", Name = "User", NormalizedName = "USER".ToUpper() });

            modelBuilder.Entity<Bar>().HasMany(r => r.Reviews).WithOne(b => b.Bar);
            modelBuilder.Entity<AppUser>().HasMany(r => r.Reviews).WithOne(a => a.User);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Bar> Bars { get; set; }

    }
}