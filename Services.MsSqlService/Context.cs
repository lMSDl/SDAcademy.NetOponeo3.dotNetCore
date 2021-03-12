using System;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.MsSqlService.Configurations;

namespace Services.MsSqlService
{
    public class Context : DbContext
    {
        //public DbSet<User> Users {get; set;}

        public Context() {
        }
        public Context(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new TireConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
