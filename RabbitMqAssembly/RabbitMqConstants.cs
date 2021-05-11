using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqAssembly
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri = "rabbitmq://localhost/";
        public const string UserName = "guest";
        public const string Password = "guest";
        
        public const string RegisterEmailSenderQueue = "emailsender.service";
        public const string RegisterEmailSenderConsumerQueue = "emailsenderconsume.service";

    }
}

