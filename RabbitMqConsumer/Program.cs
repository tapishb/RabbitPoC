using GreenPipes;
using MassTransit;
using RabbitMqAssembly;
using System;
using System.Threading.Tasks;

namespace RabbitMqConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //trail and error
            //var bus = BusConfigurator.ConfigureBus();
            //bus.ConnectReceiveEndpoint(RabbitMqConstants.RegisterEmailSenderQueue, e =>
            //{
            //    e.Consumer<MessageConsumer>();
            //}
            //);

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(RabbitMqConstants.RabbitMqUri), hst =>
                {
                    hst.Username(RabbitMqConstants.UserName);
                    hst.Password(RabbitMqConstants.Password);
                });
                cfg.ReceiveEndpoint(RabbitMqConstants.RegisterEmailSenderConsumerQueue, e =>
                {
                    e.UseMessageRetry(r => r.Immediate(10));
                    e.Consumer<MessageConsumer>();
                    e.Consumer<ResponseConsumer>();
                }
                );
            });
            await bus.StartAsync(); ;

            Console.WriteLine("Consumer Bus is running");
            Console.ReadLine();

            await bus.StopAsync();
        }
    }
}
