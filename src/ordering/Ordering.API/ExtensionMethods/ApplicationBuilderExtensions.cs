using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.API.RabbitMQ;

namespace Ordering.API.ExtensionMethods
{
    public static class ApplicationBuilderExtensions
    {
         public static EventRabbitMQConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitMQListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventRabbitMQConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}