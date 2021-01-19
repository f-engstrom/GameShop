using System;
using System.Collections.Generic;
using System.Text;
using GameShop.Models;
using Microsoft.EntityFrameworkCore;

namespace GameShop.Data
{
    class GameShopContext:DbContext
    {
        public DbSet<Address> Address { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }

        public DbSet<Product> Product { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=GameShop;Integrated Security=True");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductOrder>().HasKey(bc => new {bc.OrderId, bc.ProductId});

            modelBuilder.Entity<ProductOrder>().HasOne(bc => bc.Order)
                .WithMany(c => c.Products)
                .HasForeignKey(bc => bc.ProductId);

            modelBuilder.Entity<ProductOrder>().HasOne(bc => bc.Product)
                .WithMany(c => c.Orders)
                .HasForeignKey(bc => bc.OrderId);

        }
    }
}
