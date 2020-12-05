using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.CORE.Entities;

namespace Ordering.INFRASTRUCTURE.Data
{
    public class OrderContextSeed
    {
        public static async Task Seed(OrderContext context, ILoggerFactory logger, int? retry = 0)
        {
            int retryAvailability = retry.Value;
            try
            {
                // Migrate() needs SqlServer dependency installed from nuget
                context.Database.Migrate();
                if(!context.Orders.Any())
                    context.Orders.AddRange(GetPreloadedOrders());
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                // tries 3 times
                if(retryAvailability < 3)
                {
                    retryAvailability++;
                    var log = logger.CreateLogger<OrderContextSeed>();
                    log.LogError(exception.Message);
                    // retries after catching the error
                    await Seed(context, logger, retryAvailability);
                }
            }
        }

    private static IEnumerable<Order> GetPreloadedOrders()
    {
      return new List<Order>()
      {
          new Order() {EmailAddress = "bob@test.com", TotalPrice = 125M},
          new Order() {EmailAddress = "rob@test.com", TotalPrice = 126M},
          new Order() {EmailAddress = "joe@test.com", TotalPrice = 127M},
          new Order() {EmailAddress = "biff@test.com", TotalPrice = 128M}
      };
    }
  }
}