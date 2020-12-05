using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.CORE.Entities;
using Ordering.CORE.Repositories;
using Ordering.INFRASTRUCTURE.Data;

namespace Ordering.INFRASTRUCTURE.Repositories
{
  public class OrderRepository : Repository<Order>, IOrderRepository
  {
    public OrderRepository(OrderContext ctx) : base(ctx)
    {
    }

    public async Task<IEnumerable<Order>> GetAllByUsername(string username) =>
        await _ctx.Orders.Where(o => o.UserName == username).ToListAsync();
  }
}