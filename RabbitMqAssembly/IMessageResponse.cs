using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqAssembly
{
    public interface IMessageResponse 
    {
        Guid CorrelationId { get; }
        string MessageResponse { get; set; }
    }
}
