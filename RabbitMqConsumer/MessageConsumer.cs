using MassTransit;
using RabbitMqAssembly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqConsumer
{
    public class MessageConsumer : IConsumer<IMessage>
    {
        public async Task Consume(ConsumeContext<IMessage> context)
        {
            await Console.Out.WriteLineAsync($"MessageConsumer got it { context.Message.Text } for { context.Message.CorrelationId }");
            await Task.Delay(10000);
            await context.Publish<IMessage>(new
            {
                CorrelationId = Guid.NewGuid(),
                Text = "GOTIT"
            });
            await context.RespondAsync<IMessageResponse>(new
            {
                MessageResponse = "Got it",
                CorrelationId = context.Message.CorrelationId
            });
            //throw new Exception("blah");
        }
    }
}
