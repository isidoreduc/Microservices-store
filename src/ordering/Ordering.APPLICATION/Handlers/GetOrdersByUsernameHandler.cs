using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.APPLICATION.Mapper;
using Ordering.APPLICATION.Queries;
using Ordering.APPLICATION.Responses;
using Ordering.CORE.Repositories;

namespace Ordering.APPLICATION.Handlers
{
  public class GetOrdersByUsernameHandler : IRequestHandler<GetOrdersByUsernameQuery, IEnumerable<OrderResponse>>
  {
    private readonly IOrderRepository _orderRepository;
    public GetOrdersByUsernameHandler(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersByUsernameQuery request, CancellationToken cancellationToken)
    {
      var orderList = await _orderRepository.GetAllByUsername(request.Username);
      var orderResponseList = OrderMapper.Mapper.Map<IEnumerable<OrderResponse>>(orderList);
      return orderResponseList;
    }
  }
}