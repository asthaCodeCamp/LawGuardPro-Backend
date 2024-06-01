using LawGuardPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LawGuardPro.Infrastructure.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Case> Cases { get; set; }
    public DbSet<Lawyer> Lawyers { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<AddressUser> AddressUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Case>()
            .HasOne(c => c.ApplicationUser)
            .WithMany()
            .HasForeignKey(c => c.ApplicationUserId);

        modelBuilder.Entity<Case>()
            .HasOne(c => c.Lawyer)
            .WithMany()
            .HasForeignKey(c => c.LawyerId);

        modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.AddressUsers)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey(a => a.UserId);
    }
}
