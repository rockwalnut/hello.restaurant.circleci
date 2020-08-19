using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace hello.restaurant.api.Models
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext()
        { }

        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        { }

        //public DbSet<Blog> Blogs { get; set; }
        //public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }

        //model validate
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}