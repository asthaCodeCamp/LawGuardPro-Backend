using  LawGuardPro.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LawGuardPro.Infrastructure.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Case> Cases { get; set; }
    public DbSet<Lawyer> Lawyers { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<Address> Addresss { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Case>()
            .HasOne(c => c.ApplicationUser)
            .WithMany(u => u.Cases)
            .HasForeignKey(c => c.UserId).IsRequired();

        modelBuilder.Entity<Case>()
            .HasOne(c => c.Lawyer)
            .WithMany(l => l.Cases)
            .HasForeignKey(c => c.LawyerId);
            
        modelBuilder.Entity<Address>()
             .HasOne(a => a.ApplicationUser)
             .WithMany(u => u.AddressUsers)
             .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<Case>()
            .Property(c => c.Status)
            .HasConversion<int>();
    }
}
