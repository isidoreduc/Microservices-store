using System;
using AutoMapper;
using Ordering.APPLICATION.Commands;
using Ordering.APPLICATION.Responses;
using Ordering.CORE.Entities;

namespace Ordering.APPLICATION.Mapper
{
    // using automapper in classlib projects, no Startup class available
    public class OrderMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<OrderMappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }

  internal class OrderMappingProfile : Profile
  {
      public OrderMappingProfile()
        {
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
        }
  }
}