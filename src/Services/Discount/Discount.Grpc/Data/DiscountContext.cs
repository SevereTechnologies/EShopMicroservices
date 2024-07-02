using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = default!;

    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon() { Id = 1, ProductName = "IPhone 10", Description = "IPhone 10 Description", Amount = 800 },
            new Coupon() { Id = 2, ProductName = "Macbook M3", Description = "Macbook Description", Amount = 1500 },
            new Coupon() { Id = 3, ProductName = "Macbook M4", Description = "Macbook Description M4", Amount = 2500 }
            );
        
    }
}
