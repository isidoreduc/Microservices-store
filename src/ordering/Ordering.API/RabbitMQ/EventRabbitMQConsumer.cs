using System.Text;
using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.APPLICATION.Commands;
using Ordering.CORE.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ordering.API.RabbitMQ
{
  public class EventRabbitMQConsumer
  {
    private readonly IRabbitMQConnection _rabbitMQConnection;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IOrderRepository _orderRepository;
    public EventRabbitMQConsumer(IRabbitMQConnection rabbitMQConnection, IMapper mapper, IMediator mediator, IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository;
      _mediator = mediator;
      _mapper = mapper;
      _rabbitMQConnection = rabbitMQConnection;
    }

    public void Consume()
    {
        var channel = _rabbitMQConnection.CreateModel();
        channel.QueueDeclare(queue: "BasketCheckoutQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += ReceivedEvent;

        channel.BasicConsume(queue: "BasketCheckoutQueue", autoAck: true, consumer: consumer,exclusive: false, arguments: null);
    }

    private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == "BasketCheckoutQueue")
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

                // EXECUTION : Call Internal Checkout Operation
                var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);
                var result = await _mediator.Send(command);
            }
        }

        public void Disconnect()
        {
            _rabbitMQConnection.Dispose();
        }
  }
}