using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMQ
{
  public class RabbitMQConnection : IRabbitMQConnection
  {
    private readonly IConnectionFactory _connectionFactory;
    private IConnection _connection;
    private bool _disposed = false;

    public RabbitMQConnection(IConnectionFactory connectionFactory)
    {
      _connectionFactory = connectionFactory;
      if(!IsConnected)
        TryConnect();
    }

    public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

    public bool TryConnect()
    {
      try
      {
          _connection = _connectionFactory.CreateConnection();
      }
      catch (BrokerUnreachableException)
      {
          Thread.Sleep(2000);
          _connection = _connectionFactory.CreateConnection();
      }
      return IsConnected == true ? true : false;
    }

    public IModel CreateModel()
    {
      return IsConnected == true ?
        _connection.CreateModel() : throw new InvalidOperationException("No RabbitMq connection");
    }

    public void Dispose()
    {
        if(_disposed)
            return;
        else
            _connection.Dispose();
    }

  }
}