using Microsoft.EntityFrameworkCore;
using Ordering.CORE.Entities;

namespace Ordering.INFRASTRUCTURE.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options): base(options)
        {

        }

        public DbSet<Order> Orders { get; set;}
    }
}