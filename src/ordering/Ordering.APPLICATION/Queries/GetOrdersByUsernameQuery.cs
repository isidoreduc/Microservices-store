using System.Collections;
using System.Collections.Generic;
using MediatR;
using Ordering.APPLICATION.Responses;

namespace Ordering.APPLICATION.Queries
{
  public class GetOrdersByUsernameQuery : IRequest<IEnumerable<OrderResponse>>
  {
    public string Username { get; set; }
    public GetOrdersByUsernameQuery(string username)
    {
      Username = username;
    }
  }
}