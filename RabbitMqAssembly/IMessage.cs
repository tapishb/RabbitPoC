using System;

namespace RabbitMqAssembly
{
    public interface IMessage
    {
        Guid CorrelationId { get; set; }
        string Text { get; set; }
        string Description { get; set; }
    }
}