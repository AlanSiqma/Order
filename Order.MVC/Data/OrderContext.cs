using Microsoft.EntityFrameworkCore;
using Order.MVC.Models;

namespace Order.MVC.Data
{
    public class OrderContext : DbContext
    {
        public DbSet<OrderEntity> OrderEntity { get; set; }
        public DbSet<ProductEntity> ProductEntity { get; set; }
        public DbSet<OrderProductEntity> OrderProduct { get; set; }

        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {
        }
    }
}
