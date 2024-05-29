using LawGuardPro.Domain.Entities;
using LawGuardPro.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Lawyer> Lawyers { get; set; }
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
        }
    }
}
