using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
/// <summary>
/// The application db context.
/// </summary>

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        SeedRoles(modelBuilder);
        // Add other seed methods if needed
    }

    private void SeedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Student", NormalizedName = "STUDENT" },
            new IdentityRole { Id = "2", Name = "BBD Admin", NormalizedName = "BBD ADMIN" },
            new IdentityRole { Id = "3", Name = "University Admin", NormalizedName = "UNIVERSITY ADMIN" }
        );
    }
}


