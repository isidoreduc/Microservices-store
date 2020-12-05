using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.CORE.Entities;

namespace Ordering.CORE.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUsername(string username);
    }
}