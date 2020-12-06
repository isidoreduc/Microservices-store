using AutoMapper;
using EventBusRabbitMQ.Events;
using Ordering.APPLICATION.Commands;

namespace Ordering.API.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
        }
    }
}