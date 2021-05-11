using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqAssembly
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {                
                cfg.Host(new Uri(RabbitMqConstants.RabbitMqUri), hst =>
                {
                    hst.Username(RabbitMqConstants.UserName);
                    hst.Password(RabbitMqConstants.Password);
                });

                //cfg.ReceiveEndpoint(RabbitMqConstants.RegisterEmailSenderConsumerQueue, e =>
                //{
                //    e.PrefetchCount = 5;
                //    //e.Consumer<IMessage>();

                //});

                registrationAction?.Invoke(cfg);

            });
        }
    }
}
