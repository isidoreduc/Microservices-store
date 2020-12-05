using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.CORE.Entities;

namespace Ordering.CORE.Repositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<IEnumerable<Order>> GetAllByUsername(string username);
    }
}