using RabbitMqAssembly;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace RabbitMqSender
{

    public class Program
    {
        public static async Task Main()
        {

            var bus = BusConfigurator.ConfigureBus();
            // some trial and error
            //var bus = BusConfigurator.ConfigureBus((cfg) =>
            //{
            //    cfg.ReceiveEndpoint(RabbitMqConstants.RegisterEmailSenderQueue, e =>
            //    {
            //        e.Consumer<IMessage>();
            //    });
            //};



            //);

            //var rBus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            //{
            //    sbc.Host(RabbitMqConstants.RabbitMqUri);

            //    sbc.ReceiveEndpoint(RabbitMqConstants.RegisterEmailSenderQueue, ep =>
            //    {
            //        ep.Handler<Message>(context =>
            //        {
            //            return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
            //        });
            //    });
            //});



            await bus.StartAsync(); // This is important!

            //await bus.Publish(new Message { Text = "Hi" });

            //bus.ConnectConsumer<IMessage>();

            Console.WriteLine("Publisher Bus is running");
                        
            
            var sendToUri = new Uri($"{RabbitMqConstants.RabbitMqUri}" + $"{RabbitMqConstants.RegisterEmailSenderConsumerQueue}");
            
            var endPoint = await bus.GetSendEndpoint(sendToUri);

            await endPoint.Send<IMessage>(new Message { CorrelationId = Guid.NewGuid(), Text = "Hii" });

            await bus.Publish<IMessage>(new
            {
                CorrelationId = Guid.NewGuid(),
                Text = "Yo"
            });
                        
            var client = bus.CreateRequestClient<IMessage>(sendToUri);

            try
            {
                var response = await client.GetResponse<IMessageResponse>(new { CorrelationId = Guid.NewGuid(), Text = "Yellooo" });

                Console.WriteLine($"Press any key to exit, the message is { response.Message.MessageResponse } for { response.Message.CorrelationId }");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Press any key to exit, the exception is {e.Message}");
            }
            await Task.Run(() => Console.ReadKey());

            await bus.StopAsync();
        }
    }
}