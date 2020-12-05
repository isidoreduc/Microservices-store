using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.APPLICATION.Commands;
using Ordering.APPLICATION.Mapper;
using Ordering.APPLICATION.Responses;
using Ordering.CORE.Entities;
using Ordering.CORE.Repositories;

namespace Ordering.APPLICATION.Handlers
{
  public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, OrderResponse>
  {
    private readonly IOrderRepository _orderRepository;
    public CheckoutOrderHandler(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity =  OrderMapper.Mapper.Map<Order>(request);
        if(orderEntity == null)
            throw new ApplicationException("mapping failed");
        var newOrder = await _orderRepository.AddAsync(orderEntity);
        return OrderMapper.Mapper.Map<OrderResponse>(newOrder);
    }

  }
}