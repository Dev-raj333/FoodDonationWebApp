﻿using FoodDonationWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDonationWebApp.Data
{
    public class FoodDbContext : IdentityDbContext<ApplicationUser>
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }    
        public DbSet<Donation> Donations { get; set; }
        public DbSet<PickupRequest> PickupRequests { get; set; }
        public DbSet<DropRequest> DropRequests { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<RecipientRequest> RecipientRequests { get; set; }
        public DbSet<AllocatedFood> AllocatedFoods { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AllocatedFood>()
                .HasKey(d => d.Id);
            modelBuilder.Entity<RecipientRequest>()
                .HasOne(r => r.Recipient)
                .WithMany(u => u.RecipientRequests)
                .HasForeignKey(r => r.RecipientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
